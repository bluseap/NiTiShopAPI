var postController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    var addeditPost = new addeditPostController();   

    this.initialize = function () {
        loadCorporation();
        loadData();
        registerEvents();

        addeditPost.initialize();      
    }

    function registerEvents() {
        $('#txtKeyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                addeditPost.loadTablePost();
            }
        });

        $("#btnSearch").on('click', function () {
            addeditPost.loadTablePost();
        });

        $("#ddl-show-pagePost").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            addeditPost.loadTablePost(true);
        });

        $("#btnCreate").on('click', function () {
            addeditPost.AddEditClearData();
            // 1 - insert Post
            $('#hidInsertPost').val(1);

            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var postId = $(this).data('id');
            // 2 - Update Post
            $('#hidInsertPost').val(2);
            loadEditPost(postId);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var postId = $(this).data('id');
            deletePost(postId);
        });
    }

    function loadCorporation() {
        return $.ajax({
            type: 'GET',
            url: '/admin/Corporation/GetListCorporations',
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCorporation').html(render);
                $('#ddlAddEditCorporation').html(render);

                if (userCorporationId !== "1") {
                    $('#ddlCorporation').prop('disabled', true);
                    $('#ddlAddEditCorporation').prop('disabled', true);
                }
                else {
                    $('#ddlCorporation').prop('disabled', false);
                    $('#ddlAddEditCorporation').prop('disabled', false);
                }

                $("#ddlCorporation")[0].selectedIndex = 1;
                $("#ddlAddEditCorporation")[0].selectedIndex = 1;

                var corpoId = $('#ddlCorporation').val();

                loadCategoryNews(corpoId);
                addeditPost.loadTablePost();
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadCategoryNews(cororationsId) {
        return $.ajax({
            type: 'GET',
            url: '/admin/CategoryNews/GetCoporationId',
            data: {
                cororationId: cororationsId
            },
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response.Items, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCategorySearch').html(render);
                $('#ddlAddEditCategoryNews').html(render);

                $("#ddlCategorySearch")[0].selectedIndex = 0;  
                $("#ddlAddEditCategoryNews")[0].selectedIndex = 0;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadEditPost(postId) {
        $.ajax({
            type: "GET",
            url: "/Admin/post/GetById",
            data: {
                id: postId
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var post = response;

                $('#hidPostId').val(post.Id);

                $('#ddlAddEditCorporation').val(post.CorporationId);
                $('#ddlAddEditCategoryNews').val(post.CategoryNewsId);

                loadPostImage(post.Id);

                //addeditproduct.loadProductCatelogies(product.CategoryId);

                $('#txtAddEditTitle').val(post.Title);
                $('#txtAddEditDescription').val(post.Description);   

                $('#imagelistImagePost').html('');
                $('#imagelistImagePost').append('<div ><img width="100"  data-path="' + post.Image + '" src="' + post.Image + '" /></div>');
                $('#hidImagePost').val(post.Image);

                CKEDITOR.instances.txtAddEditContent.setData(post.Content);

                $('#txtSeoTitle').val(post.SeoTitle);
                $('#txtSeoAlias').val(post.SeoAlias);
                $('#txtSeoKeyword').val(post.SeoMetaKeywords);
                $('#txtSeoDescription').val(post.SeoMetaDescription);
                $('#txtSeoTag').val(post.SeoTags);

                $('#ckAddEditActive').prop('checked', post.IsActive);               

                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function loadPostImage(postid) {
        $.ajax({
            url: '/admin/post/GetImagePostId',
            data: {
                postId: postid
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-3"><img width="100" src="' + item.Path
                        + '" /><br/><a href="#" class="btn-delete-image" data-id="' + item.Id + '" >Xóa</a></div>';
                });

                $('#imagelistImagePost2').html(render);
            }
        });
    }

    function deletePost(postId) {
        niti.confirm(resources["DeleteSure"], function () {
            $.ajax({
                type: "POST",
                url: "/Admin/post/DeletePost",
                data: {
                    Id: postId,
                    username: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Post.");
                    niti.notify(resources["DeleteTableOK"], 'success');
                    niti.stopLoading();
                    addeditPost.loadTablePost();
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }

    function loadData() {

    }

}