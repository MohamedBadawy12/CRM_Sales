function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('show');
}

function confirmDelete(formId) {
    if (confirm('Are you sure you want to delete this item?')) {
        document.getElementById(formId).submit();
    }
}

function togglePassword(inputId, iconId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(iconId);
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.replace('bi-eye', 'bi-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.replace('bi-eye-slash', 'bi-eye');
    }
}

function getAgentsByLeader(leaderId, targetDropdownId) {
    if (!leaderId) return;
    $.ajax({
        url: '/SalesAgent/GetByLeader',
        type: 'GET',
        data: { leaderId: leaderId },
        success: function (data) {
            const dropdown = $('#' + targetDropdownId);
            dropdown.empty();
            dropdown.append('<option value="">-- Select Agent --</option>');
            $.each(data, function (i, agent) {
                dropdown.append(
                    `<option value="${agent.id}">${agent.agentName}</option>`
                );
            });
        }
    });
}
function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('show');
}