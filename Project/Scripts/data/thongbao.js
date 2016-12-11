// thong bao
$(function () {

    $('#AlertBox').removeClass('hide');
    $('#AlertBox').delay(5000).slideUp(9000);

});
//cat chuoi

$(function () {
    $('.Changetitle').keyup(function () { $('.slug').val(string_to_slug($('.Changetitle').val())); });
});

function string_to_slug(str) {
    str = str.replace(/^\s+|\s+$/g, ''); // trim
    str = str.toLowerCase();

    // remove accents, swap ñ for n, etc
    var from = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴáàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
    var to = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaeeeeeeeeeeeeeeeeeeeeeeoooooooooooooooooooooooooooooooooouuuuuuuuuuuuuuuuuuuuuuiiiiiiiiiiddyyyyyyyyyy";
    for (var i = 0, l = from.length; i < l; i++) {
        str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
    }

    str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
              .replace(/\s+/g, '-') // collapse whitespace and replace by -
              .replace(/-+/g, '-'); // collapse dashes
    return str;
}
// thay thế chuổi bằng (.....)
//$(document).ready(function () {
//    function truncateText(selector, maxLength) {
//        var element = document.querySelector(selector),
//            truncated = element.innerText;

//        if (truncated.length > maxLength) {
//            truncated = truncated.substr(0, maxLength) + '...';
//        }
//        return truncated;
//    }

//    document.querySelector('#nd1').innerText = truncateText('#nd1', 187);
//    document.querySelector('#nd2').innerText = truncateText('#nd2', 187);
//    document.querySelector('#nd3').innerText = truncateText('#nd3', 187);
//});
