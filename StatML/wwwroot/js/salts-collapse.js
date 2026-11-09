// wwwroot/js/salts-collapse.js
window.onload = function () {
    const tbody = document.querySelector('.table tbody');
    if (!tbody) return;

    const rows = Array.from(tbody.querySelectorAll('tr[data-group]'));
    if (rows.length === 0) return;

    const groupMap = new Map();
    const groupColors = ['#ffffff', '#f8f9fa' /*'#f5fbff'*/];
    let visibleIndex = 0;

    rows.forEach(row => {
        const groupId = row.getAttribute('data-group');
        if (!groupId) return;

        if (!groupMap.has(groupId)) {
            groupMap.set(groupId, { rows: [], firstRow: row, header: null });
        }
        groupMap.get(groupId).rows.push(row);
    });

    groupMap.forEach((group, groupId) => {
        const firstRow = group.firstRow;
        const coords = firstRow.cells[0].textContent.trim();
        const allPlayers = group.rows
            .slice()
            .reverse()
            .map(r => r.cells[3].querySelector('a')?.textContent.trim())
            .filter(Boolean);
        const playerCount = allPlayers.length;

        // Цвет для всей группы (включая заголовок)
        const color = groupColors[visibleIndex % 2];

        // ОДИНОЧНАЯ ЗАПИСЬ
        if (group.rows.length === 1) {
            firstRow.cells[0].innerHTML += ' <small style="color:#777;">(1 запись)</small>';
            firstRow.style.cssText = `background-color: ${color} !important;`;
            visibleIndex++;
            return;
        }

        // ГРУППА С ЗАГОЛОВКОМ
        const headerRow = document.createElement('tr');
        headerRow.className = 'group-header';
        headerRow.setAttribute('data-group-header', groupId);
        headerRow.style.cssText = `background-color: ${color} !important;`; // ← ЗАГОЛОВОК ТОГО ЖЕ ЦВЕТА

        headerRow.innerHTML = `
            <td colspan="8">
                <div class="header-content">
                    <div>
                        <span class="coords">${coords}</span>
                        <span class="players">— ${allPlayers.join(' → ')}</span>
                        <small class="count">(${playerCount} записей)</small>
                    </div>
                    <i class="fas fa-chevron-down toggle-icon"></i>
                </div>
            </td>
        `;

        tbody.insertBefore(headerRow, firstRow);
        group.header = headerRow;

        // Все строки группы — тот же цвет
        group.rows.forEach(row => {
            row.classList.add('group-detail');
            row.style.cssText = `background-color: ${color} !important; display: none;`;
        });

        // Клик
        headerRow.addEventListener('click', (e) => {
            if (e.target.closest('a, button, input')) return;
            const isOpen = headerRow.classList.toggle('open');
            const icon = headerRow.querySelector('.toggle-icon');
            icon.classList.toggle('fa-chevron-down', !isOpen);
            icon.classList.toggle('fa-chevron-up', isOpen);
            group.rows.forEach(row => {
                row.style.display = isOpen ? '' : 'none';
            });
        });

        visibleIndex++; // ← Следующая группа — другой цвет
    });
};