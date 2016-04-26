var url;
function EditClass(editUrl) {
    url = editUrl;
    this.Success = function (result) {
        if (result.success) {
            window.location.href = url;
        } else {
            for (var i = 0; i < result.errors.length; i++) {
                $('#errorMessages').append(result.errors[i] + '<br />');
            }
        }
    }
    this.Failure = function (result) {
        alert(result);
    }
}