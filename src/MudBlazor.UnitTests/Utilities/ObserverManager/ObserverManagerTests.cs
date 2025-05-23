﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MudBlazor.Utilities.ObserverManager;
using NUnit.Framework;

namespace MudBlazor.UnitTests.Utilities.ObserverManager;

#nullable enable
[TestFixture]
public class ObserverManagerTests
{
    private ObserverManager<int, string> _observerManager = new(NullLogger<ObserverManager<int, string>>.Instance);

    [SetUp]
    public void Setup()
    {
        //Reset on each test
        _observerManager = new ObserverManager<int, string>(NullLogger<ObserverManager<int, string>>.Instance);
    }

    [Test]
    public void Constructor_ThrowsException()
    {
        // Arrange & Act
        var construct = () => new ObserverManager<int, string>(null!);

        // Assert
        construct.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TryGetSubscription_ReturnsTrueAndObserver_WhenObserverExists()
    {
        // Arrange
        var id = 1;
        var observer = "Observer1";
        _observerManager.Subscribe(id, observer);

        // Act
        var result = _observerManager.TryGetSubscription(id, out var retrievedObserver);

        // Assert
        result.Should().BeTrue();
        retrievedObserver.Should().Be(observer);
    }

    [Test]
    public void TryGetSubscription_ReturnsFalseAndDefault_WhenObserverDoesNotExist()
    {
        // Arrange
        var id = 1;

        // Act
        var result = _observerManager.TryGetSubscription(id, out var retrievedObserver);

        // Assert
        result.Should().BeFalse();
        retrievedObserver.Should().BeNull();
    }

    [Test]
    public void TryGetOrAddSubscription_ReturnsTrueAndUpdatesObserver_WhenObserverExists()
    {
        // Arrange
        var id = 1;
        var observer1 = "Observer1";
        var observer2 = "Observer2";
        _observerManager.Subscribe(id, observer1);

        // Act
        var result = _observerManager.TryGetOrAddSubscription(id, observer2, out var newObserver);

        // Assert
        result.Should().BeTrue();
        newObserver.Should().Be(observer2);
        _observerManager.Count.Should().Be(1);
        _observerManager.Observers[id].Should().Be(observer2);
    }

    [Test]
    public void TryGetOrAddSubscription_ReturnsFalseAndAddsObserver_WhenObserverDoesNotExist()
    {
        // Arrange
        var id = 1;
        var observer = "Observer1";

        // Act
        var result = _observerManager.TryGetOrAddSubscription(id, observer, out var newObserver);

        // Assert
        result.Should().BeFalse();
        newObserver.Should().Be(observer);
        _observerManager.Count.Should().Be(1);
        _observerManager.Observers[id].Should().Be(observer);
    }

    [Test]
    public void FindObserverIdentities_ReturnsMatchingIdentities()
    {
        // Arrange
        var observer1 = "Observer1";
        var observer2 = "Observer2";
        var observer3 = "Observer3";

        _observerManager.Subscribe(1, observer1);
        _observerManager.Subscribe(2, observer2);
        _observerManager.Subscribe(3, observer3);

        // Act
        var result = _observerManager.FindObserverIdentities((_, observer) => observer.Contains('2')).ToList();

        // Assert
        result.Should().ContainSingle().Which.Should().Be(2);
    }

    [Test]
    public void Subscribe_AddsObserverToDictionary()
    {
        // Arrange
        var id = 1;
        var observer = "Observer1";

        // Act
        _observerManager.Subscribe(id, observer);

        // Assert
        _observerManager.Count.Should().Be(1);
        _observerManager.Observers[id].Should().Be(observer);
    }

    [Test]
    public void Subscribe_UpdatesExistingObserverInDictionary()
    {
        // Arrange
        var id = 1;
        var observer1 = "Observer1";
        var observer2 = "Observer2";

        // Act
        _observerManager.Subscribe(id, observer1);
        _observerManager.Subscribe(id, observer2);

        // Assert
        _observerManager.Count.Should().Be(1);
        _observerManager.Observers[id].Should().Be(observer2);
    }

    [Test]
    public void Unsubscribe_RemovesObserverFromDictionary()
    {
        // Arrange
        var id = 1;
        var observer = "Observer1";
        _observerManager.Subscribe(id, observer);

        // Act
        _observerManager.Unsubscribe(id);

        // Assert
        _observerManager.Count.Should().Be(0);
        _observerManager.IsSubscribed(id).Should().BeFalse();
    }

    [Test]
    public async Task NotifyAsync_CallsNotificationForEachObserver()
    {
        // Arrange
        var observer1 = "Observer1";
        var observer2 = "Observer2";
        var notificationCalledCount = 0;

        _observerManager.Subscribe(1, observer1);
        _observerManager.Subscribe(2, observer2);

        // Act
        async Task NotificationAsync(string _)
        {
            notificationCalledCount++;
            await Task.Delay(10); // Simulate some async work
        }

        await _observerManager.NotifyAsync(NotificationAsync);

        // Assert
        notificationCalledCount.Should().Be(2);
    }

    [Test]
    public async Task NotifyAsync_RemovesDefunctObservers()
    {
        // Arrange
        var observer1 = "Observer1";
        var observer2 = "Observer2";
        var observer3 = "Observer3";

        _observerManager.Subscribe(1, observer1);
        _observerManager.Subscribe(2, observer2);
        _observerManager.Subscribe(3, observer3);

        // Act
        Task NotificationAsync(string observer)
        {
            if (observer == observer2)
            {
                throw new Exception("Notification failed");
            }

            return Task.CompletedTask;
        }

        await _observerManager.NotifyAsync(NotificationAsync);

        // Assert
        _observerManager.Count.Should().Be(2);
        _observerManager.IsSubscribed(1).Should().BeTrue();
        _observerManager.IsSubscribed(3).Should().BeTrue();
        _observerManager.IsSubscribed(2).Should().BeFalse();
    }

    [Test]
    public void Clear_ClearsAllObservers()
    {
        // Arrange
        _observerManager.Subscribe(1, "Observer1");
        _observerManager.Subscribe(2, "Observer2");
        _observerManager.Subscribe(3, "Observer3");

        // Act
        _observerManager.Clear();

        // Assert
        _observerManager.Count.Should().Be(0);
        _observerManager.Observers.Should().BeEmpty();
    }

    [Test]
    public void GetEnumerator_ReturnsAllObservers()
    {
        // Expected
        var expectedObservers = new List<string> { "Observer1", "Observer2", "Observer3" };

        // Arrange
        for (var i = 0; i < expectedObservers.Count; i++)
        {
            _observerManager.Subscribe(i + 1, expectedObservers[i]);
        }

        // Act
        var actualObservers = new List<string>();
        using (var enumerator = _observerManager.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                actualObservers.Add(enumerator.Current);
            }
        }

        // Assert
        actualObservers.Should().BeEquivalentTo(expectedObservers);
    }

    [Test]
    public void GetEnumeratorNonGeneric_ReturnsAllObservers()
    {
        // Expected
        var expectedObservers = new List<string> { "Observer1", "Observer2", "Observer3" };

        // Arrange
        for (var i = 0; i < expectedObservers.Count; i++)
        {
            _observerManager.Subscribe(i + 1, expectedObservers[i]);
        }

        // Act
        var actualObservers = new List<string>();
        var enumerator = ((IEnumerable)_observerManager).GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is string observer)
            {
                actualObservers.Add(observer);
            }
        }

        // Assert
        actualObservers.Should().BeEquivalentTo(expectedObservers);
    }

    [Test]
    public void IEnumerable_GetEnumerator_ReturnsAllObservers()
    {
        // Expected
        var expectedObservers = new List<string> { "Observer1", "Observer2", "Observer3" };

        // Arrange
        for (var i = 0; i < expectedObservers.Count; i++)
        {
            _observerManager.Subscribe(i + 1, expectedObservers[i]);
        }

        // Act
        var actualObservers = _observerManager.ToList();

        // Assert
        actualObservers.Should().BeEquivalentTo(expectedObservers);
    }


    [Test]
    public void Unsubscribe_Subscribe_UpdateSubscribe_TraceLogEnabled_LogsDebugInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        loggerMock.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);

        var observerManager = new ObserverManager<int, string>(loggerMock.Object);

        const int Id = 1;
        const string Observer = "Observer1";

        // Act
        observerManager.Subscribe(Id, Observer);
        observerManager.Subscribe(Id, Observer);
        observerManager.Unsubscribe(Id);

        // Assert
        loggerMock
            .VerifyLogging($"Adding entry for {Id}/{Observer}. 1 total observers after add.", LogLevel.Trace)
            .VerifyLogging($"Updating entry for {Id}/{Observer}. 1 total observers.", LogLevel.Trace)
            .VerifyLogging($"Removed entry for {Id}. 0 total observers after remove.", LogLevel.Trace);
    }

    [Test]
    public async Task NotifyAsync_DefunctObserver_LogsTraceInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        loggerMock.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);

        var observerManager = new ObserverManager<int, string>(loggerMock.Object);

        const int DefunctObserverId = 1;
        const string DefunctObserver = "DefunctObserver";

        observerManager.Subscribe(DefunctObserverId, DefunctObserver);

        bool Predicate(int id, string observer) => observer == DefunctObserver;

        async Task NotificationAsync(string observer)
        {
            await Task.Delay(10); // Simulating some asynchronous operation
            throw new Exception("Simulated exception");
        }

        // Act
        await observerManager.NotifyAsync(NotificationAsync, Predicate);

        // Assert
        loggerMock
            .VerifyLogging($"Adding entry for {DefunctObserverId}/{DefunctObserver}. 1 total observers after add.", LogLevel.Trace)
            .VerifyLogging($"Removing defunct entry for {DefunctObserverId}. 0 total observers after remove.", LogLevel.Trace);
    }

    [Test]
    public void CollectionModified()
    {
        // Arrange
        var observerManager = new ObserverManager<int, int>(NullLogger.Instance);

        for (var i = 0; i < 1000; i++)
        {
            observerManager.Subscribe(i, i);
        }

        bool Predicate(int id, int observer)
        {
            if (id == 500)
            {
                observerManager.Subscribe(1001, 1001);
            }

            return true;
        }

        Task NotificationAsync(int observer)
        {
            return Task.CompletedTask;
        }

        // Act & Assert
        Assert.DoesNotThrowAsync(() => observerManager.NotifyAsync(NotificationAsync, Predicate));
    }
}
