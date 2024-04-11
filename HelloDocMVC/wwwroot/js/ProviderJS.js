

function DashboardMain() {
    let action = "/Provider" + "/" + "LoadPartialDashboard"
    $.ajax({
        url: action,
        type: 'GET',
        data: { req: 1 },
        success: function (result) {
            $('#home-tab-pane').html(result);
            $("#providerTabMain").removeClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}

function MyProfileMain() {
    let action = "/Provider" + "/" + "myProfile"
    $.ajax({
        url: action,
        type: 'GET',
        data: {statusId : 2},
        success: function (result) {
            console.log(result)
            $('#Provider-profile-tab-pane').html(result);
            $("#providerTabMain").removeClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function (e) {
            console.log(e);
            alert('Error loading partial view');
        }
    });
}

function ScheduleMain() {
    $.ajax({
        url: "/Provider/GetScheduling",
        type: 'GET',
        success: function (result) {
            $("#ProSchedule-tab-pane").html(result);
            document.getElementById("Providers-tab").classList.remove("active");
            $("#providerTabMain").addClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function () {
            alert("Error");
        },
    });
}
function newTabTwo() {
    var action = "/Provider" + "/" + "newTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [1] },
        success: function (result) {
            event.preventDefault();
            $('#myTabContent2').html(result);
            $("#new-tab").addClass("show active")
            $("#Pending-tab").removeClass("show active")
            $("#Active-tab").removeClass("show active")
            $("#Conclude-tab").removeClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function pendingTabTwo() {
    var action = "/Provider" + "/" + "pendingTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [2] },
        success: function (result) {
            event.preventDefault();
            $('#myTabContent2').html(result);
            $("#Pending-tab").addClass("show active")
            $("#new-tab").removeClass("show active")
            $("#Active-tab").removeClass("show active")
            $("#Conclude-tab").removeClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function activeTabTwo() {
    var action = "/Provider" + "/" + "activeTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [4, 5] },
        success: function (result) {
            event.preventDefault();
            $('#myTabContent2').html(result);
            $("#Active-tab").addClass("show active")
            $("#Pending-tab").removeClass("show active")
            $("#new-tab").removeClass("show active")
            $("#Conclude-tab").removeClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function concludeTabTwo() {
    var action = "/Provider" + "/" + "concludeTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [6] },
        success: function (result) {
            $('#myTabContent2').html(result);
            $("#Conclude-tab").addClass("show active");
            $("#Active-tab").removeClass("show active")
            $("#Pending-tab").removeClass("show active")
            $("#new-tab").removeClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


//$("#new-tab").on('click', function (event) {
//    if (document.getElementById("new-tab").classList.contains("active")) {
//        document.getElementById("triangle1").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})

//$("#Pending-tab").on('click', function (event) {
//    if (document.getElementById("Pending-tab").classList.contains("active")) {
//        document.getElementById("triangle2").style.display = "block"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("new-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})

//$("#Active-tab").on('click', function (event) {

//    if (document.getElementById("Active-tab").classList.contains("active")) {
//        document.getElementById("triangle3").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("new-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})

//$("#Conclude-tab").on('click', function (event) {
//    if (document.getElementById("Conclude-tab").classList.contains("active")) {
//        document.getElementById("triangle4").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("new-tab").classList.remove("active")
//    }
//})

//document.getElementById("Pending-tab").addEventListener('click', function () {
//    if (document.getElementById("Pending-tab").classList.contains("active")) {
//        document.getElementById("triangle2").style.display = "block"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("new-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})

//document.getElementById("new-tab").addEventListener('click', function () {
//    if (document.getElementById("new-tab").classList.contains("active")) {
//        document.getElementById("triangle1").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})
//document.getElementById("Active-tab").addEventListener('click', function () {
//    if (document.getElementById("Active-tab").classList.contains("active")) {
//        document.getElementById("triangle3").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("triangle4").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("new-tab").classList.remove("active")
//        document.getElementById("Conclude-tab").classList.remove("active")
//    }
//})
//document.getElementById("Conclude-tab").addEventListener('click', function () {
//    if (document.getElementById("Conclude-tab").classList.contains("active")) {
//        document.getElementById("triangle4").style.display = "block"
//        document.getElementById("triangle2").style.display = "none"
//        document.getElementById("triangle3").style.display = "none"
//        document.getElementById("triangle1").style.display = "none"
//        document.getElementById("Pending-tab").classList.remove("active")
//        document.getElementById("Active-tab").classList.remove("active")
//        document.getElementById("new-tab").classList.remove("active")
//    }
//})


