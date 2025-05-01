// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/categories')
        .then(response => {
            if (!response.ok) throw new Error("Failed to load categories.");
            return response.json();
        })
        .then(categories => {
            const dropdown = document.getElementById('categoryDropdown');
            dropdown.innerHTML = ""; // Clear loading state

            categories.forEach(name => {
                const li = document.createElement("li");
                const link = document.createElement("a");
                link.className = "dropdown-item text-black";
                link.href = `/Equipment?category=${encodeURIComponent(name)}`;
                link.textContent = name;
                li.appendChild(link);
                dropdown.appendChild(li);
            });
        })
        .catch(error => {
            console.error("Dropdown load error:", error);
            document.getElementById('categoryDropdown').innerHTML =
                '<li class="dropdown-item text-danger">Failed to load categories</li>';
        });
});