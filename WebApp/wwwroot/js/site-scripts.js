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

    if (message && type) {
        const icon = document.getElementById("modalIcon");
        const msgText = document.getElementById("modalMessage");

        msgText.textContent = message;

        if (type === "success") {
            icon.className = "fas fa-check-circle text-success mb-3";
        } else {
            icon.className = "fas fa-exclamation-circle text-danger mb-3";
        }

        const modal = new bootstrap.Modal(document.getElementById("generalMessageModal"));
        modal.show();

        // Auto close after 1.5 seconds
        setTimeout(() => {
            modal.hide();
        }, 1500);
    }
});

