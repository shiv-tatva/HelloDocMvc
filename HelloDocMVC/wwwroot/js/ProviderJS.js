function DashboardMain() {
    let action = "/Provider" + "/" + "LoadPartialDashboard"
    $.ajax({
        url: action,
        type: 'GET',
        data: { req: 1 },
        success: function (result) {
            $('#content').html(result);
            $("#providerTabMain").removeClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function ScheduleMain() {
    $.ajax({
        url: "/Provider/GetScheduling",
        type: 'GET',
        success: function (result) {
            $("#content").html(result);
            document.getElementById("Providers-tab").classList.remove("active");
            $("#providerTabMain").addClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        },
    });
}

function MyProfileMain() {
    let action = "/Provider" + "/" + "myProfile"
    $.ajax({
        url: action,
        data: { statusId : 2},
        type: 'GET',
        success: function (result) {
            $('#content').html(result);
            $("#providerTabMain").removeClass("customactive");
            $("#accessTabMain").removeClass("customactive");
            $("#recordMainTab").removeClass("customactive");
        },
        error: function (e) {
            console.log(e);
            toastr.error("Error To Load Partial View");
        }
    });
}

function InvoicingMain() {
    let action = "/Provider" + "/" + "Invoicing"
    $.ajax({
        url: action,
        type: 'GET',
        success: function (result) {
            $('#content').html(result);
        },
        error: function (e) {
            console.log(e);
            toastr.error("Error To Load Partial View");
        }
    });
}

function newTabThree() {
    document.getElementById("triangle1").style.display = "block"
    document.getElementById("triangle2").style.display = "none"
    document.getElementById("triangle3").style.display = "none"
    document.getElementById("triangle4").style.display = "none"
    document.getElementById("Pending-tab").classList.remove("active")
    document.getElementById("Active-tab").classList.remove("active")
    document.getElementById("Conclude-tab").classList.remove("active")

    var action = "/Provider" + "/" + "newTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [1], dataFlag : 11},
        success: function (result) {
            $('#myTabContent1').html(result);
            $("#New-tab-pane").addClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}



function pendingTabThree() {

    document.getElementById("triangle1").style.display = "none"
    document.getElementById("triangle2").style.display = "block"
    document.getElementById("triangle3").style.display = "none"
    document.getElementById("triangle4").style.display = "none"
    document.getElementById("new-tab").classList.remove("active")
    document.getElementById("Active-tab").classList.remove("active")
    document.getElementById("Conclude-tab").classList.remove("active")

    var action = "/Provider" + "/" + "pendingTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [2], dataFlag : 12 },
        success: function (result) {
            $('#myTabContent1').html(result);
            $("#Pending-tab-pane").addClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function activeTabThree() {

    document.getElementById("triangle1").style.display = "none"
    document.getElementById("triangle2").style.display = "none"
    document.getElementById("triangle3").style.display = "block"
    document.getElementById("triangle4").style.display = "none"
    document.getElementById("Pending-tab").classList.remove("active")
    document.getElementById("new-tab").classList.remove("active")
    document.getElementById("Conclude-tab").classList.remove("active")

    var action = "/Provider" + "/" + "activeTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [4, 5], dataFlag : 13 },
        success: function (result) {
            $('#myTabContent1').html(result);
            $("#Active-tab-pane").addClass("show active")
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function concludeTabThree() {

    document.getElementById("triangle1").style.display = "none"
    document.getElementById("triangle2").style.display = "none"
    document.getElementById("triangle3").style.display = "none"
    document.getElementById("triangle4").style.display = "block"
    document.getElementById("Pending-tab").classList.remove("active")
    document.getElementById("Active-tab").classList.remove("active")
    document.getElementById("new-tab").classList.remove("active")

    var action = "/Provider" + "/" + "concludeTabTwo"
    $.ajax({
        url: action,
        type: 'GET',
        traditional: true,
        contentType: "application/json",
        data: { arr: [6], dataFlag : 14},
        success: function (result) {
            $('#myTabContent1').html(result);
            $("#Conclude-tab-pane").addClass("show active");
        },
        error: function () {
            alert('Error aa loading partial view');
        }
    });
}


function newViewNote(reqid) {
    console.log(reqid)
    $.ajax({
        url: "/Provider/newViewNote",
        data: { data: reqid },
        type: 'GET',
        success: function (result) {
            $("#content").html(result);
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        }
    });
}



function newViewCase(reqid) {
    console.log(reqid)
    $.ajax({
        url: "/Provider/newViewCase",
        data: { data: reqid },
        type: 'GET',
        success: function (result) {
            $("#content").html(result);
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        }
    });
}