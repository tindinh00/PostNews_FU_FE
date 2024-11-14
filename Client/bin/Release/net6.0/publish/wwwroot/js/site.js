(function () {
    "use strict";

    /**
     * Easy selector helper function
     */
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    /**
     * Easy event listener function
     */
    const on = (type, el, listener, all = false) => {
        if (all) {
            select(el, all).forEach(e => e.addEventListener(type, listener))
        } else {
            select(el, all).addEventListener(type, listener)
        }
    }

    /**
     * Easy on scroll event listener 
     */
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    /**
     * Sidebar toggle
     */
    if (select('.toggle-sidebar-btn')) {
        on('click', '.toggle-sidebar-btn', function (e) {
            select('body').classList.toggle('toggle-sidebar')

        })
    }

    /**
     * Navbar links active state on scroll
     */
    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    /**
     * Toggle .header-scrolled class to #header when page is scrolled
     */
    let selectHeader = select('#header')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
            } else {
                selectHeader.classList.remove('header-scrolled')
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    /**
     * Back to top button
     */
    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    /**
     * Initiate tooltips
     */
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })


    ///**
    // * Initiate Bootstrap validation check
    // */
    //var needsValidation = document.querySelectorAll('.needs-validation')

    //Array.prototype.slice.call(needsValidation)
    //    .forEach(function (form) {
    //        form.addEventListener('submit', function (event) {
    //            if (!form.checkValidity()) {
    //                event.preventDefault()
    //                event.stopPropagation()
    //            }

    //            form.classList.add('was-validated')
    //        }, false)
    //    })

})();

/*--------------------------------------------------------------
# Loader
--------------------------------------------------------------*/
//Show preloader when loading page
//$(document).ready(function () {
//    // Show the preloader when the page is ready
//    document.getElementById("main").classList.add("loading-skeleton");
//    //showLoader();
//});

//// Hide the preloader when the page is fully loaded
//document.onreadystatechange = function () {
//    if (document.readyState === 'complete') {
//        document.getElementById("main").classList.remove("loading-skeleton");
//        //hideLoader();
//    }
//};

$(document).ready(function () {
    // Show the preloader when the page is ready
    $(".main img").each(function () {
        $(this).addClass("loading-skeleton");
    });
});

// Hide the preloader when the page is fully loaded
$(window).on('load', function () {
    $(".main img").each(function () {
        $(this).removeClass("loading-skeleton");
    });
});

// Function to show the loader
function showLoader() {
    document.getElementById("loader").style.display = "flex";
}

// Function to hide the loader
function hideLoader() {
    document.getElementById("loader").style.display = "none";
}


/*--------------------------------------------------------------
# Others 
--------------------------------------------------------------*/
// Function for update selected campus
function updateSelectedCampus(e) {

    var selectedId = e.value;

    fetch('/Campus/SetSelectedCampus?selectedCampusId=' + selectedId)
        .then(response => {
            if (response.ok) {
                console.log("Update selected campus successful!");
                window.location.reload();
            }
        })
        .catch(error => {
            console.error('Update selected campus error:', error);
        });
}

function updateSelectedEduSystem(e) {

    var selectedId = e.value;

    fetch('/EduSystem/SetSelectedEduSystem?selectedEduSystemId=' + selectedId)
        .then(response => {
            if (response.ok) {
                console.log("Update selected campus successful!");
                window.location.reload();
            }
        })
        .catch(error => {
            console.error('Update selected campus error:', error);
        });
}


function previewAvatarImage(input, id) {
    var preview = document.getElementById(id);
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            preview.src = e.target.result; // Set the source of the preview image
        };

        reader.readAsDataURL(input.files[0]); // Read the selected file as a data URL
    }
}

function previewFile(input, id) {
    var submitBtn = document.getElementById('btn-submit');
    if (input.files && input.files[0]) {
        const file = input.files[0];
        const filePreview = document.getElementById(id);

        filePreview.addEventListener('load', function () {
            // Now you can enable the button or perform other actions
            submitBtn.disabled = false;
        });

        filePreview.src = URL.createObjectURL(file);

        filePreview.style.display = "block";
    }
}

function onChooseFile(id) {
    document.getElementById(id).click(); // Trigger file input click event
}

function submitForm(id) {
    document.getElementById(id).submit();
}

function handleKeyPressSubmitForm(event, id) {
    if (event.keyCode === 13) {
        submitForm(id);
    }
}

function showSpinner() {
    var spinner = document.getElementById("spinner");
    var btnContent = document.getElementById("btn-content");
    btnContent.className = "visually-hidden";
    spinner.className = "";
    setTimeout(() => {
        spinner.parentElement.disabled = true;
    }, 100);
}

function showSpinnerCustom(spinnerId, buttonId) {
    var spinner = document.getElementById(spinnerId);
    var btnContent = document.getElementById(buttonId);
    btnContent.className = "visually-hidden";
    spinner.className = "";

    setTimeout(() => {
        spinner.parentElement.disabled = true;
    }, 100);
}


function setCurrentPage(pageIndex) {
    var currentPage = document.getElementById('current-page');
    currentPage.value = pageIndex;

    var formSearch = document.getElementById('form-search');
    formSearch.submit();
}


function showToast(type, message) {
    toastr[type](message);
}


function onChangeEduSystem(isSubmitId, formId) {
    var isSubmit = document.getElementById(isSubmitId);
    isSubmit.value = "false";
    submitForm(formId);
}


function onClearSearchTerm(id) {
    var input = document.getElementById(id);
    input.value = "";
}

function isGoogleFormLink(link) {
    // Check if the link contains the keywords "google.com/forms" or "google.com/url?q=forms"
    return (
        link.includes("google.com/forms") || link.includes("google.com/url?q=forms")
    );
}

function isLink(link) {
    return (link.includes("https://"));
}


function goBackToPreviousPage() {
    history.back();
}

function setImageViewUrl(url) {
    document.getElementById('image-view').src = url;
}