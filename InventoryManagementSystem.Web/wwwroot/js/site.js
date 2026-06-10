// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.downloadFile = (filename, contentType, content) => {
    const file = new File([content], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);
    const a = document.createElement("a");
    a.href = exportUrl;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(exportUrl);
}
