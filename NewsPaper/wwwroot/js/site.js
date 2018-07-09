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
        path: "/lib/editor.md/lib/"
       
    });
});
$(".new").click(function () {
    $.get();
});
