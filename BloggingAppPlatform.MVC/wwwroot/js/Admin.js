document.addEventListener('DOMContentLoaded', function () {
    const navLinks = document.querySelectorAll('#sidebar .nav-link');
    const currentPath = window.location.pathname;

    // Set the active class based on the current URL
    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });

    // Handle click events to update the active link
    navLinks.forEach(link => {
        link.addEventListener('click', function () {
            // Remove 'active' class from all links
            navLinks.forEach(l => l.classList.remove('active'));

            // Add 'active' class to the clicked link
            this.classList.add('active');
        });
    });
});