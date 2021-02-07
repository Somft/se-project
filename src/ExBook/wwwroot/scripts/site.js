jQuery.validator.setDefaults({
    highlight: function (element) {
        jQuery(element).closest('.form-control').addClass('is-invalid');
    },
    unhighlight: function (element) {
        jQuery(element).closest('.form-control').removeClass('is-invalid');
    },
    errorElement: 'span',
    errorClass: 'label label-danger',
});

$(function () {
    $("#submitLogout").click(function () {
        document.getElementById("logoutForm").submit();
        return false;
    });
});