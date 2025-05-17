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


    // Handle reset button to clear fields and auto-submit
    form.addEventListener("reset", function (e) {
        // Wait for the reset to apply
        setTimeout(() => {
            // Clear all select values explicitly (optional, since reset usually does this)
            form.querySelectorAll("select").forEach(select => {
                select.selectedIndex = 0;
            });

            // Clear all text inputs
            form.querySelectorAll("input[type='text']").forEach(input => {
                input.value = "";
            });

            // Submit the form after reset
            form.submit();
        }, 0);
    });
});

