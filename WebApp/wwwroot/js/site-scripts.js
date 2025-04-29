function previewImage(event) {
    const file = event.target.files[0];
    const uploadBox = document.querySelector('.upload-box');
    const uploadtxt = document.querySelector('.upload-text');

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            uploadBox.style.backgroundImage = `url('${e.target.result}')`;
            uploadBox.style.backgroundSize = 'cover';
            uploadBox.style.backgroundPosition = 'center';
            uploadtxt.innerHTML = ''; // Remove the "Add" text
        };
        reader.readAsDataURL(file);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const message = document.getElementById("tempDataMessage")?.value;
    const type = document.getElementById("tempDataMessageType")?.value;
    const targetId = document.getElementById("deleteTargetId")?.value;

    if (message && type) {
        const icon = document.getElementById("modalIcon");
        const msgText = document.getElementById("modalMessage");
        const questionActions = document.getElementById("questionActions");
        const modal = new bootstrap.Modal(document.getElementById("generalMessageModal"));

        msgText.textContent = message;

        if (type === "success") {
            icon.className = "fas fa-check-circle text-success mb-3";
        } else if (type === "error") {
            icon.className = "fas fa-times-circle text-danger mb-3";
        } else if (type === "warning") {
            icon.className = "fas fa-exclamation-triangle text-warning mb-3";
        } else if (type === "question") {
            icon.className = "fas fa-question-circle text-info mb-3";

            // Show Yes/Cancel buttons
            questionActions.classList.remove("d-none");
            document.getElementById("confirmId").value = targetId;
            document.getElementById("confirmForm").action = "/Equipment/SetInactive";
        }

        modal.show();

        if (type !== "question") {
            setTimeout(() => {
                modal.hide();
            }, 2500);
        }
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const priceInput = document.querySelector("input[name='RentalPricePerDay']");
    if (priceInput) {
        priceInput.addEventListener("input", function (e) {
            this.value = this.value.replace(/[^0-9.]/g, '');
        });
    }
});


