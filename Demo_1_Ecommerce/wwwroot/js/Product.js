var dataTable;
$(document).ready(function(){
    LoadData();
});

function LoadData(){
    dataTable = $("#productTable").DataTable({
        "ajax": {
            "url":"/Admin/Product/getData"
        },
        "columns": [
            {"data":"name"},
            { "data":"description"},
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                    <a onClick=deleteItem("/Admin/Product/DeleteProduct/${data}") class="btn btn-danger">Delete</a>
                    `
                }
            }
        ]
    });
}

function deleteItem(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toaster.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }
                }
            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}