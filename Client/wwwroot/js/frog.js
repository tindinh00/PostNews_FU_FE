const target = document.getElementById('toggle-sidebar-target'); // The target div
const sidebar = document.getElementById('sidebar'); // The target div
const screenWidth = window.innerWidth;

const toggleSidebarBtn = document.getElementById('toggle-sidebar-btn');

let isFrogSit = false;

toggleSidebarBtn.addEventListener('click', function () {
    // change position of frog
    isFrogSit = !isFrogSit;

    // get size of sidebar
    const sidebarDivRect = sidebar.getBoundingClientRect();

    // get distance from sidebar to target
    let distance = getDistance(target, sidebar);

    if (screenWidth > 1200) {
        if (isFrogSit == true) {
            distance += 100;
        } else {
            distance = Math.abs(sidebarDivRect.left) - 10;
        }

        // Set the new left position
        toggleSidebarBtn.style.left = distance + 'px';
    } else {
        distance = sidebarDivRect.width - 10;

        if (Math.abs(sidebarDivRect.x) != 300) {
            distance = 0;
        }

        toggleSidebarBtn.style.left = distance + 'px';
    }
});


function getDistance(element1, element2) {
    const rect1 = element1.getBoundingClientRect();
    const rect2 = element2.getBoundingClientRect();

    const distance = Math.abs(rect2.right - rect1.left);

    return distance;
}