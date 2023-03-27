console.log('CUSTOM JS INIT');

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


function customScroll() {
    let customScroll = document.querySelector('.custom-select');
    let customScrollBtn = document.querySelectorAll('.custom-select button');
    for (let i = 0; i < customScrollBtn.length; i++) {
        customScrollBtn[i].addEventListener('click', () => {
            customScroll.scrollTop = 0;
            console.log(customScrollBtn);
        });
    }
}

//function hideModal() {
//    let bellBtn = document.querySelector('.oi.oi-bell');
//    let modalBody = document.querySelector('.modal');
//    let modal = document.querySelector('.modal-notification');
//    let body = document.querySelector('body');

//    bellBtn.addEventListener('click', () => {
//        modalBody.style.display = 'block';
//    });

//    body.addEventListener('click', (e) => {
//        if (e.target == modalBody) {
//            modalBody.style.display = 'none';
//        } else if (e.target == modal) {
//            e.preventDefault();
//        }
//    });
//}

//let body = document.querySelector('body');
//body.addEventListener('click', () => {
//    function validPhone() {
//        let phone = document.querySelectorAll('[type="tel"]');
//        for (let i = 0; i < phone.length; i++) {
//            phone[i].addEventListener('focusout', (e) => {
//                let phoneVal = e.target.value;

//                newval = phoneVal.replace(/[^\d]/g, '');
//                e.target.value = newval;
//                e.target.setAttribute('data-value', e.target.value);
//                console.log(e.target.value);
//            });

//        }
//    }

//    validPhone();
//});
