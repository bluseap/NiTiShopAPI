
var editpassController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    this.initialize = function () {

        registerEvents();

        $('#txtEditCurrentPassword').prop('disabled', true);

    }

    function registerEvents() {

        formMainValidate();

        $('#btnSaveEditPass').on('click', function (e) {
            var insertUser = $('#hidInsertUser').val();
            if (insertUser === "2") {                
                updateUserPass(e);
            }
            else {
                niti.notify("Thêm mới lỗi!", "error");
            }
        });           

    }

    function formMainValidate() {
        jQuery.validator.addMethod("isCompobox", function (value, element) {
            if (value === "%" || value === "0")
                return false;
            else
                return true;
        },
            "Chọn !"
        );

        jQuery.validator.addMethod("isDateVietNam", function (value, element) {
            return this.optional(element) || moment(value, "DD/MM/YYYY").isValid();
        },
            resources["DD/MM/YYYY"]
        );

        //Init validation
        $('#frmMainEditPassword').validate({
            errorClass: 'red',
            ignore: [],
            language: 'vi',
            rules: {                
                txtEditNewPassword: { required: true }               
            }
        });
    }

    function updateUserPass(e) {
        e.preventDefault();
        var userid = $('#hidUserId').val();
       
        if ($('#frmMainEditPassword').valid()) {      
            
            var newpassword = $('#txtEditNewPassword').val();

            $.ajax({
                type: "POST",
                url: "/Admin/AppUser/UpdateUserPass",
                data: {
                    Id: userid,                    
                    PasswordHash: newpassword
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update User Password.");
                    niti.notify("Thêm mới thành công", 'success');
                    $('#modal-edit-password').modal('hide');                   

                    niti.stopLoading();
                },
                error: function () {
                    niti.notify("Thêm mới lỗi.", 'error');
                    niti.stopLoading();
                }
            });

        }

    }



}