(function () {
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
        },

        deleteQuillText: function (quillElement, maxLen) {

            let htmlContent = QuillFunctions.removeEmptySpan(quillElement.__quill.root.innerHTML);
            var cnt = 0;
            if (htmlContent.length > maxLen) {
                // Convert Quill content into a Delta object
                let delta = quillElement.__quill.getContents();
                
                // Start trimming from the end until HTML is within limits
                while (QuillFunctions.removeEmptySpan(quillElement.__quill.root.innerHTML).length > maxLen && delta.ops.length > 0) {
                    cnt += 1;
                    var d = QuillFunctions.removeEmptySpan(quillElement.__quill.root.innerHTML).length - maxLen;
                    var z = d % 100;
                    for (let i = 0; i < d / 1000; i++) {
                        delta.ops.pop(); // Remove the last operation
                    }
                    
                    quillElement.__quill.setContents(delta);
                }
            }
            return QuillFunctions.getQuillContent(quillElement);
            //quillElement.__quill.deleteText(maxLen, quillElement.__quill.getLength() - maxLen)   
        },
        removeEmptySpan: function (incomingHTML) {
            var test = "abcdabcdabcd";
            test = test.replaceAll("a", "g");
            return incomingHTML.replaceAll('<span class="ql-ui" contenteditable="false"></span>', "");
        }
    };
})();