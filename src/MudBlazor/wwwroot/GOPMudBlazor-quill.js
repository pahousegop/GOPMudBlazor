﻿(function () {
    window.QuillFunctions = {        
        createQuill: function (
            quillElement, id, readOnly,
            placeholder, theme, formats, debugLevel, dotNetRef) {  

            //Quill.register('modules/blotFormatter', QuillBlotFormatter.default);
            //const toolbarOptions = [['bold', 'italic', 'underline', 'strike'],
            //                    [{ 'background': ["", "#FCC832"] }]];
            var options = {
                debug: debugLevel,
                modules: {
                    toolbar: '#QuillToolbar_' + id
                },
                placeholder: placeholder,
                readOnly: readOnly,
                theme: theme,
                bounds: '#QuillEditor_' + id
            };

            if (formats) {
                options.formats = formats;
            }

            quillElement.__quill = new Quill(quillElement, options);

            quillElement.__quill.on("text-change", function () {
                dotNetRef.invokeMethodAsync("HandleContentChange");
            });
        },
        getQuillContentChunk: function (quillElement, i, chunkSize) {
            let content = JSON.stringify(quillElement.__quill.getContents());

            return content.substring(i, i + chunkSize); // Returns an array of HTML chunks
        },
        getQuillContent: function(quillElement) {
            return JSON.stringify(quillElement.__quill.getContents());
        },
        getQuillText: function(quillElement) {
            return quillElement.__quill.getText();
        },
        getQuillHTMLChunk: function (quillElement, i, chunkSize) {
            let htmlContent = quillElement.__quill.root.innerHTML;
           
            return htmlContent.substring(i, i + chunkSize); // Returns an array of HTML chunks
        },
        getQuillHTML: function(quillElement) {
            return quillElement.__quill.root.innerHTML;
        },
        loadQuillContent: function(quillElement, quillContent) {
            content = JSON.parse(quillContent);
            return quillElement.__quill.setContents(content, 'api');
        },
        loadQuillHTMLContent: function (quillElement, quillHTMLContent) {
            return quillElement.__quill.root.innerHTML = quillHTMLContent;
        },
        enableQuillEditor: function (quillElement, mode) {
            quillElement.__quill.enable(mode);
        },
        insertQuillImage: function (quillElement, imageURL) {
            var Delta = Quill.import('delta');
            editorIndex = 0;

            if (quillElement.__quill.getSelection() !== null) {
                editorIndex = quillElement.__quill.getSelection().index;
            }

            return quillElement.__quill.updateContents(
                new Delta()
                    .retain(editorIndex)
                    .insert({ image: imageURL },
                        { alt: imageURL }));
        },
        insertQuillText: function (quillElement, text) {
            editorIndex = 0;
            selectionLength = 0;

            if (quillElement.__quill.getSelection() !== null) {
                selection = quillElement.__quill.getSelection();
                editorIndex = selection.index;
                selectionLength = selection.length;
            }

            return quillElement.__quill.deleteText(editorIndex, selectionLength)
                .concat(quillElement.__quill.insertText(editorIndex, text));
        }
    };
})();