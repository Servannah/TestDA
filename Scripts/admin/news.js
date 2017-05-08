
//tạo slug khi gõ vào tên tiêu đề
$(document).ready(function () {
    //tạo thư mục upload ảnh sử dụng ckfinder
    $('#chooseImage').click(function (e) {
        e.preventDefault();
        var finder = new CKFinder();
        ////
        finder.selectActionFunction = function (url, data) {
            //console.log(file[0]['value'].data);
            console.log(data);
            var selectedFile = data['fileUrl'];
            if (selectedFile) {
                var FileSize = 0;
                if (data['fileSize'] > 1024) {
                    FileSize = Math.round(data['fileSize'] * 100 / 1024) / 100 + " MB";
                }
                else {
                    FileSize = data['fileSize'] + " KB";
                }
                // here we will add the code of thumbnail preview of upload images
                $("#FileSize").text(FileSize);
            }
            $("#Imagecontainer").empty();
            var img = new Image()
            img.src = selectedFile;
            img.className = "thumb";
            $("#Imagecontainer").append(img);
            //hiển thị đường dẫn ảnh lên textbox
            $('#UploadedFile').val(url);
        }
        finder.popup();
    });
    //hiển thị slug vào ô textbox
    $('#title-news').change(function () {
        var title = $('#title-news').val();
        console.log(title);
        $.ajax({
            type: "POST",
            url: "/News/Slug",
            data: { titleNews: title },
            dataType: "json",
            success: function (data) {
                $('#slug-news').val(data);
            },
            error: function (error) {

            }
        });
    });
        //hiển thị kích thước ảnh khi lấy dữ liệu thông tin một bài viết
        var tmpImg = new Image();
        var image = $('#newsAvatar').attr('src');
        tmpImg.src = image;
        $(tmpImg).one('load', function () {
            // orgWidth = tmpImg.width;
            // orgHeight = tmpImg.height;
            var size_image = Math.round((getImageSizeInBytes(image) / 1024), 1);
            if (size_image > 1024) {
                $('#FileSize').text(size_image/1024 + " MB");
            }else{
                console.log(size_image);
                $('#FileSize').text(size_image + " KB");
            }
        
        });
        function getImageSizeInBytes(imgURL) {
            var request = new XMLHttpRequest();
            request.open("HEAD", imgURL, false);
            request.send(null);
            var headerText = request.getAllResponseHeaders();
            var re = /Content\-Length\s*:\s*(\d+)/i;
            re.exec(headerText);
            return parseInt(RegExp.$1);
        }
    //kết thúc hiển thị kích thước ảnh
});

////script upload file
//$(document).ready(function () {
//    $('#UploadedFile').change(function () {
//        //var selectedFile = evt.target.files can use this  or select input file element
//        //and access it's files object
//        var selectedFile = ($("#UploadedFile"))[0].files[0];//FileControl.files[0];
//        if (selectedFile) {
//            var FileSize = 0;
//            var imageType = /image.*/;
//            if (selectedFile.size > 1048576) {
//                FileSize = Math.round(selectedFile.size * 100 / 1048576) / 100 + " MB";
//            }
//            else if (selectedFile.size > 1024) {
//                FileSize = Math.round(selectedFile.size * 100 / 1024) / 100 + " KB";
//            }
//            else {
//                FileSize = selectedFile.size + " Bytes";
//            }
//            // here we will add the code of thumbnail preview of upload images

//            $("#FileName").text("Name : " + selectedFile.name);
//            $("#FileType").text("Type : " + selectedFile.type);
//            $("#FileSize").text("Size : " + FileSize);
//        }

//        if (selectedFile.type.match(imageType)) {
//            var reader = new FileReader();
//            reader.onload = function (e) {

//                $("#Imagecontainer").empty();
//                var dataURL = reader.result;
//                var img = new Image()
//                img.src = dataURL;
//                img.className = "thumb";
//                $("#Imagecontainer").append(img);
//            };
//            reader.readAsDataURL(selectedFile);
//        }
//    });
//});