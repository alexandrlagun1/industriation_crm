function tabsSection() {

    let tabButtons = document.querySelectorAll('.tab');
    let tabs = document.querySelectorAll('.tab-section');

    for (i = 0; i < tabButtons.length; i++) {
        let tabBtn = tabButtons[i];
        let tabSection = tabs[i];

        tabBtn.addEventListener('click', () => {

            for (j = 0; j < tabButtons.length; j++) {
                tabButtons[j].classList.remove('tab-active');
                tabs[j].classList.remove('tab-section-active');
            }

            tabBtn.classList.add('tab-active');
            tabSection.classList.add('tab-section-active');
        });
    }
};

$('body').on('focus', 'input[type="text"]', function() {

    var mask_option = {phone: '+{7}(000)000-00-00'};
 

    if($(this).hasClass('phoneMasked'))
        setMaskI(mask_option['phone'], $(this));

});

function setMaskI(_mask, _this) {

    _this_classes = _this[0].className.split(' ');
    _js_query = '.' + _this_classes.join('.');

    var maskOptions = {
        mask: _mask
    };
    var mask = IMask(document.querySelector(_js_query), maskOptions);

    //console.log('unmasked value => ', mask.unmaskedValue);
    
}



    

