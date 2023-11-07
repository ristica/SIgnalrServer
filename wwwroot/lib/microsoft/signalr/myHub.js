"use strict";

toastr.options = {
    "closeButton": true,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

// -- SERVER

// 1 - establishes connection to the hub on load
var connection = new signalR.HubConnectionBuilder().withUrl("/signatureHub").build();

// 2 - post file to controller
document.getElementById("btnUpload")
        .addEventListener("click", function (event) {

    $("#success").hide();
    $("#error").hide();

    var data = new FormData();
    data.append("formFile", $("#uploadFile")[0].files[0]);

    $.ajax({
        type: 'post',
        url: "/home/savefileondisk",
        data: data,
        processData: false,
        contentType: false
    })
    .done(function (result) {
        if (result.status === "success") {
            $("#success").show();
            $("#error").show();
            $("#success").html("File uploaded successfully");
            $("#fileNameSpan").html(result.fileName);
            $("#fileSizeSpan").html(result.fileSize);
            $("#filePathSpan").html(result.filePath);

            toastr.success("Successfully uploaded!", { timeOut: 5000 });

            console.log(result.filePath);

        } else if (result.status === "error") {
            $("#error").html("File upload failed");
            $("#fileNameSpan").html("");
            $("#fileSizeSpan").html("");

            toastr.error("File upload failed", { timeOut: 5000 });
        } else {
            $("#error").html("Error !!!");
            $("#fileNameSpan").html("");
            $("#fileSizeSpan").html("");

            toastr.error("Error occurred", { timeOut: 5000 });
        }
    });
        });  

// 3 - broadcast message to all clients
document.getElementById("btnBroadcast")
    .addEventListener("click", function (event) {

        try {
            connection.invoke("BroadcastMessage", $("#filePathSpan").html());
        } catch (err) {
            console.log(err);
        }

    });



// -- CLIENT

// 4 - subscribe to notifications
document.getElementById("btnConnect")
        .addEventListener("click", function (event) {

            connection.start()
                .then(function () {
                    $("#connectionStatusSpan").html("CONNECTED");
                    $("#connectionStatusSpan").css("color", "green");
                    toastr.success("Successfully connected!", { timeOut: 5000 });
                })
                .catch(function (err) {
                    $("#connectionStatusSpan").html("NOT CONNECTED");
                    $("#connectionStatusSpan").css("color", "red");
                    toastr.error("Failed to connect to hub!", { timeOut: 5000 });
                });
        });  

// 5 - listening ...
connection.on(
    "ReceiveMessage", // message id 
    function (file) {
        $("#receivedFileNameSpan").html(file);
    });