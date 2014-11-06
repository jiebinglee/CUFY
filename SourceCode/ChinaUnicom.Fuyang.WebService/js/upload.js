var FreshiUpload = $.extend({
    UploadServerAddress: "../upload.ashx",
    UploadFile: function (importType,inputFile) {
        var fd = new FormData();
        fd.append("importType", importType);
        fd.append("fileToUpload", document.getElementById(inputFile).files[0]);

        var xhr = new XMLHttpRequest();

        /* event listners */
        //xhr.upload.addEventListener("progress", FreshiUpload.UploadProgress, false);
        xhr.addEventListener("load", FreshiUpload.UploadComplete, false);
        xhr.addEventListener("error", FreshiUpload.UploadFailed, false);
        xhr.addEventListener("abort", FreshiUpload.UploadCanceled, false);
        /* Be sure to change the url below to the url of your upload server side script */
        xhr.open("POST", FreshiUpload.UploadServerAddress);
        xhr.send(fd);
    },    
    UploadComplete: function (evt) {
        /* This event is raised when the server send back a response */
        //alert(evt.target.responseText);
        var uploadResult = verifyJsonString(evt.target.responseText);

        CreditManagement.Import(uploadResult.ImportType, uploadResult.FileName);
    },
    UploadFailed: function (evt) {
        alert("There was an error attempting to upload the file.");
    },
    UploadCanceled: function (evt) {
        alert("The upload has been canceled by the user or the browser dropped the connection.");
    },
    UploadProgress: function (evt) {
        if (evt.lengthComputable) {
            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
            document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
        }
        else {
            document.getElementById('progressNumber').innerHTML = 'unable to compute';
        }
    },
    FileSelected: function () {
        var file = document.getElementById('fileToUpload').files[0];
        if (file) {
            var fileSize = 0;
            if (file.size > 1024 * 1024)
                fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
            else
                fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';

            document.getElementById('fileName').innerHTML = 'Name: ' + file.name;
            document.getElementById('fileSize').innerHTML = 'Size: ' + fileSize;
            document.getElementById('fileType').innerHTML = 'Type: ' + file.type;
        }
    }
})