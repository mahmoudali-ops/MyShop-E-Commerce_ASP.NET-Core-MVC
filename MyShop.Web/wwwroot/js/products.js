var dtable;
$(document).ready(function () {
    loadddata();
});

function loadddata() {
    dtable = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData"
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Product/Edit/${data}" class="btn btn-sm btn-success">Edit</a>
                    <a onClick=DeleteItem("/Admin/Product/Delete/${data}") class="btn btn-sm btn-danger">Delete</a>
                    `;
                }
            }
        ]
    });
}

//function DeleteItem(url) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: "Delete ",
//                success: function (data) {
//                    dtable.ajax.reload();
//                },
//                error: function (err) {
//                    if (data.success) {
//                        dtable.ajax.reload();
//                        toaster.success(data.message);
//                    } else
//                    {
//                        toaster.error(data.message);
//                    }
//                }
//            });

//            Swal.fire({
//                title: "Deleted!",
//                text: "Your file has been deleted.",
//                icon: "success"
//            });
//        }
//    });

//}

function DeleteItem(url) {

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
                type: "DELETE",
                success: function (data) {
                    dtable.ajax.reload();
                },
                error: function (err) {
                    if (data.success) {
                        dtable.ajax.reload();
                        toaster.success(data.message);
                    } else {
                        toaster.error(data.message);
                    }
                }
            });
        }
        if (result.isConfirmed) {
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}

