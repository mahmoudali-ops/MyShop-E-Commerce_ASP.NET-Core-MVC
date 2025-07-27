var dtable;
$(document).ready(function () {
    loadddata();
});
function loadddata() 
{
    dtable = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalCost" },
            {
                "data": "id",
                "render": function (data) {
                    return "<a href='/Admin/Order/Details?orderid=" + data + "' class='btn btn-sm btn-success'>Details</a>";
                }
            }
        ]
    });
}


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
                    if (err.responseJSON?.success) {
                        dtable.ajax.reload();
                        toaster.success(err.responseJSON.message);
                    } else {
                        toaster.error(err.responseJSON?.message || "An error occurred.");
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

