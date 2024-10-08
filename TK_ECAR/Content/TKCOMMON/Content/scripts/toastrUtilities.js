/* msg : mensaje*/
/* type : s= success, i=info, e=error w=warning*/
function notify(msg, type) {
    
    toastr.clear();
    toastr.options = {
        positionClass: "toast-middle-center"

    };
    if (type == "s")
        var notify = toastr.success(msg);
    if (type == "i")
        var notify = toastr.info(msg);
    if (type == "e")
        var notify = toastr.error(msg);
    if (type == "w")
        var notify = toastr.warning(msg);


}