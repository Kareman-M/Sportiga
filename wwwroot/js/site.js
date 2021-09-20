// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder().withUrl("/Notification");

connection.on("ReseiveNotification", function () {

    var notif = " فى انتظار الموافقة على مقالة ";
    var li = document.createElement("li");
    li.textContent = notif;

});
connection.start();
$('#SendNotification').on("click", function () {

    connection.invoke("SendNotification");

})