////$(function ()
////{
////    var PlaceHolderElement = $('#PlaceHolderHere');
////    $('button[data-toggle="ajax-modal"]').click(function (e) {

////        var url = $(this).data('url');
////        $.get(url).done(function (data) {
////            PlaceHolderElement.html(data);
////            PlaceHolderElement.find('.model').modal('show');

////        })
////    })

////    PlaceHolderElement.on('click', '[data-save="modal"]', function (e) {

////        var form = $(this).parent('.modal').find('form');
////        var actionUrl = form.attr('action');
////        var sendData = form.serialize();
////        $.post(actionUrl, sendData).done(function (data) {
////            PlaceHolderElement.find('.modal').modal('hide');
////        })

////    })
////})