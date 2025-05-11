//< !--INTERACTIVITY -->
document.addEventListener("DOMContentLoaded", function () {
    // Activate tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Enable row click navigation
    document.querySelectorAll(".clickable-row").forEach(function (row) {
        row.addEventListener("click", function () {
            window.location.href = this.dataset.href;
        });
    });
});


document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("auto-filter-form");

    if (!form) return;

    // Auto-submit on change for all select elements
    form.querySelectorAll("select").forEach(select => {
        select.addEventListener("change", () => form.submit());
    });

    // Auto-submit on Enter key in all text inputs
    form.querySelectorAll("input[type='text']").forEach(input => {
        input.addEventListener("keypress", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                form.submit();
            }
        });
    });
});

