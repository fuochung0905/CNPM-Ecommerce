$(function () {
    $('p').click(function () {
        var clicked = $('p').click();
        if (clicked) { $('p').css('color', 'rgb(137, 5, 5)'); }
    });
});