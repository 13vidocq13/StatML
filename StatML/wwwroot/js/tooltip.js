document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.farm-zone-cell').forEach(cell => {
        const tooltip = cell.querySelector('.farm-tooltip');
        if (!tooltip) return;

        let showTimer, hideTimer;

        const show = () => {
            clearTimeout(hideTimer);
            showTimer = setTimeout(() => tooltip.classList.add('show'), 200);
        };

        const hide = () => {
            clearTimeout(showTimer);
            hideTimer = setTimeout(() => tooltip.classList.remove('show'), 150);
        };

        cell.addEventListener('mouseenter', show);
        cell.addEventListener('mouseleave', hide);

        // Мобильные
        let touchTimer;
        cell.addEventListener('touchstart', (e) => {
            e.preventDefault();
            touchTimer = setTimeout(show, 600);
        }, { passive: false });

        cell.addEventListener('touchend', () => {
            clearTimeout(touchTimer);
            setTimeout(hide, 2500);
        });
    });
});