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
