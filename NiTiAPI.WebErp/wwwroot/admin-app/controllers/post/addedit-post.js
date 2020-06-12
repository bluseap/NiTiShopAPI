
var addeditPostController = function () {

    var userName = $("#hidUserName").val();
    var imagePostTitle = [];
    var imagePostTitle2 = [];
    var postImgesList = [];

    this.loadTablePost = function (isPageChanged) {
        loadTablePost(isPageChanged);
    }

    this.AddEditClearData = function () {
        AddEditClearData();
    }

    this.loadCategoryNews= function (selectedId) {
        loadCategoryNews(selectedId);
    }

    this.initialize = function () {        
        registerEvents();
    }

    function registerEvents() {

        formMainValidate();

        registerControls();

        $('#btnAddEditSave').on('click', function (e) {
            var insertPost = $('#hidInsertPost').val();
            if (insertPost === "1") {
                savePost(e);
            }
            else if (insertPost === "2") {
                updatePost(e);
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });  

        $("#fileInputImagePost").on('change', function () {            
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();

            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }

            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImagePost",               
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearfileImagePost($("#fileInputImagePost"));
                    imagePostTitle.push(path);

                    $('#imagelistImagePost').append('<div class="col-md-3"><img width="100"  data-path="' + path + '" src="' + path + '"></div>');
                    niti.notify(resources["UploadFile"], 'success');
                },
                error: function () {
                    niti.notify(resources["UploadFileError"], 'error');
                }
            });
        });

        $("#fileInputImagePost2").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;

            var data = new FormData();

            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }

            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImagePost",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearfileImagePost2($("#fileInputImagePost2"));
                    imagePostTitle2.push(path);                    

                    $('#imagelistImagePost2').append('<div class="col-md-3"><img width="200"  data-path="' + path + '" src="' + path + '"></div>');

                    var insertPost = $('#hidInsertPost').val();
                    if (insertPost === "1") {
                        postImgesList.push({
                            Id: 0,
                            Path: path,
                            PostId: 0
                        });                       
                    }
                    else if (insertPost === "2") {
                        saveImageManagement(path);
                    }
                    else {
                        niti.notify(resources["CreateTableError"], "error");
                    }

                    niti.notify(resources["UploadFile"], 'success');
                },
                error: function () {
                    niti.notify(resources["UploadFileError"], 'error');
                }
            });
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div').remove();

            var postsImagesId = $(this).data("id");

            $.ajax({
                url: '/admin/post/DeletePostImage',
                data: {
                    Id: postsImagesId,
                    username: userName
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Images Posts at Image Post Management.");                    
                    //$('#imagelistImagePost2').html('');
                    clearfileImagePost2($("#fileInputImagePost2"));
                }
            });
        });

    }

    function saveImageManagement(imageManagement) {
        var postid = $('#hidPostId').val();
        $.ajax({
            url: '/admin/Post/SavePostsImages',
            data: {
                postId: postid,
                images: imageManagement,
                username: userName
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                niti.appUserLoginLogger(userName, "Save Images Post at Posts.");              
                //$('#imagelistImagePost2').html('');
                clearfileImagePost2($("#fileInputImagePost2"));
                imageManagement = [];
            }
        });
    }

    function registerControls() {
        CKEDITOR.replace('txtAddEditContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }    

    function loadTablePost(isPageChanged) {
        var corporationid = $('#ddlCorporation').val();
        var catalogid = $('#ddlCategorySearch').val();

        $.ajax({
            type: "GET",
            url: "/admin/post/GetPaging",
            data: {
                keyword: $('#txtKeyword').val(),
                culture: "vi-VN",
                corporationId: corporationid,
                categoryNewsId: catalogid,
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize               
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templatePost').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Title: item.Title,
                            CategoryNewsName: item.CategoryNewsName,
                            CorporationName: item.CorporationName,                            
                            Image: item.Image === null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.Image + '" width=25 />',
                            Status: niti.getStatusPosts(item.Status)
                        });
                    });

                    $("#lblTotalRecords").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentPost').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingPost(response.TotalRow, function () {
                            loadTablePost();
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentPost').html('');
                }
                niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingPost(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULPost a').length === 0 || changePageSize === true) {
            $('#paginationULPost').empty();
            $('#paginationULPost').removeData("twbs-pagination");
            $('#paginationULPost').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULPost').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '< ',
            prev: '<< ',
            next: '>> ',
            last: '> ',
            onPageClick: function (event, p) {
                if (niti.configs.pageIndex !== p) {
                    niti.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }

    function clearfileImagePost(ctrl) {
        try {
            imagePostTitle = [];
            ctrl.value = null;
            ctrl.value('');
        }
        catch (status) {
            console.log(status);
        }
    }

    function clearfileImagePost2(ctrl) {
        try {
            imagePostTitle2 = [];
            ctrl.value = null;
            ctrl.value('');
        }
        catch (status) {
            console.log(status);
        }
    }

    function savePost(e) {
        e.preventDefault();

        if ($('#frmMaintainanceAddEdit').valid()) {
            var corporationId = $('#ddlAddEditCorporation').val();
            var categoryNewsId = $('#ddlAddEditCategoryNews').val();
            var title = $('#txtAddEditTitle').val();
            var description = $('#txtAddEditDescription').val();            

            //var hidimageProduct = $('#hidImageProduct').val();

            var content = CKEDITOR.instances.txtAddEditContent.getData();
            var seoTitle = $('#txtSeoTitle').val();
            var seoAlias = $('#txtSeoAlias').val();
            var seoKeyword = $('#txtSeoKeyword').val();
            var seoDescripts = $('#txtSeoDescription').val();
            var seoTags = $('#txtSeoTag').val();

            var activePost = $('#ckAddEditActive').prop('checked') === true ? true : false;
                        
            var xml = '';
            xml = xml + "<tables>";
            for (var i = 0; i < postImgesList.length; i++) {
                var listfield = postImgesList[i];
                xml += "<items>";
                xml += '<Id>' + listfield.Id + '</Id>';
                xml += '<Path>' + listfield.Path + '</Path>';
                xml += '<PostId>' + listfield.PostId + '</PostId>';              
                xml += "</items>";
            }
            xml = xml + '</tables>';

            $.ajax({
                type: "POST",
                url: "/Admin/Post/CreatePostImageXML",
                data: {
                    CorporationId: corporationId,
                    CategoryNewsId: categoryNewsId,
                    Title: title,
                    Description: description,
                    
                    Image: imagePostTitle,

                    Content: content,
                    SeoTitle: seoTitle,
                    SeoAlias: seoAlias,
                    SeoMetaKeywords: seoKeyword,
                    SeoMetaDescription: seoDescripts,
                    SeoTags: seoTags,

                    listImageXML: xml,

                    IsActive: activePost,
                    LanguageId: "vi-VN",
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Save Post.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTablePost(true);
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }
    }

    function updatePost(e) {
        e.preventDefault();

        if ($('#frmMaintainanceAddEdit').valid()) {
            var postId = $('#hidPostId').val();

            var corporationId = $('#ddlAddEditCorporation').val();
            var categoryNewsId = $('#ddlAddEditCategoryNews').val();
            var title = $('#txtAddEditTitle').val();
            var description = $('#txtAddEditDescription').val();

            var hidimageProduct = $('#hidImagePost').val();

            var content = CKEDITOR.instances.txtAddEditContent.getData();
            var seoTitle = $('#txtSeoTitle').val();
            var seoAlias = $('#txtSeoAlias').val();
            var seoKeyword = $('#txtSeoKeyword').val();
            var seoDescripts = $('#txtSeoDescription').val();
            var seoTags = $('#txtSeoTag').val();

            var activePost = $('#ckAddEditActive').prop('checked') === true ? true : false;

            $.ajax({
                type: "POST",
                url: "/Admin/Post/UpdatePost",
                data: {
                    Id: postId,
                    CorporationId: corporationId,
                    CategoryNewsId: categoryNewsId,
                    Title: title,
                    Description: description,

                    Image: imagePostTitle.length > 0 ? imagePostTitle : hidimageProduct,

                    Content: content,
                    SeoTitle: seoTitle,
                    SeoAlias: seoAlias,
                    SeoMetaKeywords: seoKeyword,
                    SeoMetaDescription: seoDescripts,
                    SeoTags: seoTags,

                    IsActive: activePost,
                    LanguageId: "vi-VN",
                    UpdateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update Post.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTablePost(true);
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }
    }

    function AddEditClearData() {
        $('#hidInsertPost').val('');

        $('#ddlAddEditCorporation')[0].selectedIndex = 1;        
        $('#ddlAddEditCategoryNews')[0].selectedIndex = 0; 

        //loadCategoryNews("");

        $('#txtAddEditTitle').val("");
        $('#txtAddEditDescription').val("");      

        clearfileImagePost($("#fileInputImagePost"));
        imagePostTitle = [];

        $('#txtSeoTitle').val("");
        $('#txtSeoAlias').val("");
        $('#txtSeoKeyword').val("");
        $('#txtSeoDescription').val("");
        $('#txtSeoTag').val("");

        $('#ckAddEditActive').prop('checked', true);        
    }

    function formMainValidate() {
        jQuery.validator.addMethod("isCompobox", function (value, element) {
            if (value === "%" || value === "0" || value === "")
                return false;
            else
                return true;
        },
            resources["PleaseSelect"]
        );

        jQuery.validator.addMethod("isDateVietNam", function (value, element) {
            return this.optional(element) || moment(value, "DD/MM/YYYY").isValid();
        },
            resources["DD/MM/YYYY"]
        );

        //Init validation
        $('#frmMaintainanceAddEdit').validate({
            errorClass: 'red',
            ignore: [],
            language: 'vi',
            rules: {
                ddlAddEditCategoryNews: {
                    isCompobox: true
                },
                txtAddEditTitle: {
                    required: true
                },
                txtAddEditDescription: {
                    required: true
                },
                txtAddEditContent: {
                    required: true
                }
            }
        });
    }
    
    function loadCategoryNews(selectedId) {

    }

}