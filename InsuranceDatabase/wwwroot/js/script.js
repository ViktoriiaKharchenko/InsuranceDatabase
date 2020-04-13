//код блять 

//проверяю что сайт загрузился
$(document).ready(() => {
    //слайд сейчас
    var slideNum = 1;
    //всего слайдов #автоматизация 
    var slideCount = $('#images').children().length;
    window.setInterval(function () {
        if (slideNum == slideCount || slideNum <= 0 || slideNum > slideCount) {
            //переход на 1 слайд
            $('#images').css('transform', 'translate(0, 0)');
            slideNum = 1;
        } else {
            translateWidth = ($('#image1').width() * (slideNum));
            translateWidth = translateWidth * (-1);
            //немного магии которая вращает мир 
            $('#images').css({
                'transform': 'translate(' + translateWidth + 'px, 0)', '-webkit-transform': 'translate(' + translateWidth + 'px, 0)', '-ms-transform': 'translate(' + translateWidth + 'px, 0)',
            });
            slideNum++;
        }
        // задержка на сколько хочешь 
    }, 5000);

})