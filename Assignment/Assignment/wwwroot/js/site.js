// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function addStudent() {
    $.ajax({
        url: "/Home/AddStudentModal",
        type: 'GET',
        success: function (result) {
            console.log(result)
            $('#addStudentModal').html(result);
            $('#addStudentModalView').modal('show');
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        }
    });
}

function addStudentModal() {
    event.preventDefault();
    var formdata = $("#addStudentForm").serialize();
    if ($("#addStudentForm").valid()) {
        $.ajax({
            method: "POST",
            url: "/Home/AddStudentModal",
            data: formdata,
            success: function (res) {
                if (res) {
                    $("#addStudentModalView").modal("hide");
                    toastr.success("Student Added Successfully");
                    window.location.reload();
                } else {
                    toastr.error("Something Wrong In Creating Shift");
                }
            },
            error: function () {
                toastr.error("Error To Load Partial View");
            },
        });
    }
}


function editStudentDetails(StudentId) {
    $.ajax({
        url: "/Home/EditStudentDetails",
        type: 'GET',
        data: { studentId: StudentId },
        success: function (result) {
            console.log(result)
            $('#editStudentModal').html(result);
            $('#editStudentModalView').modal('show');
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        }
    });
}


function editStudentForm() {
    event.preventDefault();
    var formdata = $("#editStudentForm").serialize();
    if ($("#editStudentForm").valid()) {
        $.ajax({
            method: "POST",
            url: "/Home/EditStudentDetails",
            data: formdata,
            success: function (res) {
                if (res) {
                    $("#editStudentModalView").modal("hide");
                    toastr.success("Edit Date Successfully");
                    window.location.reload();
                } else {
                    toastr.error("Something Wrong In Creating Shift");
                }
            },
            error: function () {
                toastr.error("Error To Load Partial View");
            },
        });
    }
}

function deleteStudentDetail(StudentId) {
    $.ajax({
        url: "/Home/DeletePopup",
        data: { studentId: StudentId },
        type: 'GET',
        success: function (result) {
            $('#DeletePopup').html(result);
            $('#DeletePopupModal').modal('show');
        },
        error: function (e) {
            console.log(e);
            toastr.error("Error");
        },
    });
}

function DeleteConfirm(deleteId) {
    var formdata = $("#DeleteConfirmForm").serialize();
    $.ajax({
        url: "/Home/DeletePopup",
        type: 'POST',
        data: formdata,
        success: function (result) {
            console.log(result)
            $('#editStudentModal').html(result);
            $('#editStudentModalView').modal('show');
            toastr.success("Data Deleted Successfully");
            window.location.reload();
        },
        error: function () {
            toastr.error("Error To Load Partial View");
        }
    });
}

$(document).ready(function () {

    $('.SchoolManagementData').DataTable({
        "initComplete": function (settings, json) {

            $('#searchTab').val(settings.oPreviousSearch.sSearch);

            $('#searchTab').on('keyup', function () {
                var searchValue = $(this).val();
                settings.oPreviousSearch.sSearch = searchValue;
                settings.oApi._fnReDraw(settings);
            });
        },
        "lengthMenu": [[5, 10, -1], [5, 10, "All"]],
        "pageLength": 9,
        "order": [[0, "desc"]],
        language: {
            oPaginate: {
                sNext: '<i class="bi bi-caret-right-fill text-info"></i>',
                sPrevious: '<i class="bi bi-caret-left-fill text-info"></i>'

            }
        }
    });
    $('.dataTables_filter').hide();
});
