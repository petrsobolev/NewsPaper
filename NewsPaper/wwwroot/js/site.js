// Write your JavaScript code.

function getTime(id, time) {
    $("#"+id).html(moment(time).startOf('hour').fromNow());
}
var testEditormd
$(function () {
    testEditor = editormd("test-editormd", {
        width: "100%",
        height: 640,
        syncScrolling: "single",
        path: "/lib/editor.md/lib/",
        toolbarIcons: function () {
            return ["undo", "redo", "|", "bold", "del", "italic", "quote", "|", "h1", "h2", "h3", "h4", "h5", "h6", "|", "list-ul", "list-ol", "hr", "|", "link", "image", "|", "table", "code", "||", "watch", "preview", "testIcon"]
        }
       
    });
});
$(".new").click(function () {
    $.get();
});



