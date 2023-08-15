var index = 1;
$(document).ready(function () {
    // Metronic.init(); // init metronic core componets
    // Layout.init(); // init layout
    // Demo.init(); // init demo(theme settings page)
    // Index.init(); // init index page
    //TableAdvanced.init();
    //FormiCheck.init(); // init page demo
    //UIConfirmations.init(); // init page demo
    // Tasks.initDashboardWidget(); // init tash dashboard widget

    //    initialize();
    //setInterval(function () {
    //    getPending();
    //    $("#header_notification_bar span.badge").html($('.myList li').length);


    //}, 5000);

    $(".multiframeworks").select2({
        placeholder: "اطار العمل ",
        allowClear: true
    });
    $(".time_picker").timepicker();
    $('.loademail').hide();
    $('.erroremail').hide();
    $('.successemail').hide();
    $('#pac-input').attr("placeholder", "البحـث");
    $(".countrylist,.cityList,.frameworkList,.nationalityList").tagsinput({
        trimValue: true,
        allowDuplicates: false,

    });

    $('.countrylist').on('itemAdded', function (event) {
        $('#countrylist').val($('.countrylist').tagsinput('items'));
    });
    $('.countrylist').on('itemRemoved', function (event) {
        $('#countrylist').val($('.countrylist').tagsinput('items'));
    });

    $('.cityList').on('itemAdded', function (event) {
        $('#cityList').val($('.cityList').tagsinput('items'));
    });
    $('.cityList').on('itemRemoved', function (event) {
        $('#cityList').val($('.cityList').tagsinput('items'));
    });


    $('.frameworkList').on('itemAdded', function (event) {
        $('#frameworkList').val($('.frameworkList').tagsinput('items'));
    });
    $('.frameworkList').on('itemRemoved', function (event) {
        $('#frameworkList').val($('.frameworkList').tagsinput('items'));
    });


    $('.nationalityList').on('itemAdded', function (event) {
        $('#nationalityList').val($('.nationalityList').tagsinput('items'));
    });
    $('.nationalityList').on('itemRemoved', function (event) {
        $('#nationalityList').val($('.nationalityList').tagsinput('items'));
    });

    var feeds = $('#feeds').hasClass("feeds");

    if (feeds) {
        getresult('/ajax/getArchive?pageIndex=' + pageIndex);
    }
    $("input[name='Email']").blur(function () {
        var email = $(this).val();
        // console.log(email);
        $.ajax({
            url: '/ajax/checkemail?email=' + $(this).val(),
            type: "GET",
            beforeSend: function () {
                $('.loademail').show();
            },
            complete: function () {
                $('.loademail').hide();
            },
            success: function (data) {

                if (data == "False" && validateEmail(email)) {

                    $('.erroremail').hide();
                    $('.successemail').show();


                }
                else {
                    var $validator = $("#admin_form").validate();
                    /* Build up errors object, name of input and error message: */
                    errors = { Email: "المستخدم موجود مسبقاً" };
                    /* Show errors on the form */
                    $validator.showErrors(errors);
                    $('.erroremail').show();
                    $('.successemail').hide();

                }

            },
            error: function () { }
        });

    });
    $("input[name='CurrentPassword']").blur(function () {
        var email = $(this).val();
        // console.log(email);
        $.ajax({
            url: '/ajax/checkPassword?password=' + $(this).val(),
            type: "GET",
            beforeSend: function () {
                $('.loademail').show();
            },
            complete: function () {
                $('.loademail').hide();
            },
            success: function (data) {

                if (data == "False") {

                    var $validator = $("#change_pass").validate();
                    errors = { CurrentPassword: "كلمة المرور غير متطابقة" };
                    /* Show errors on the form */
                    $validator.showErrors(errors);
                    $('.erroremail').show();
                    $('.successemail').hide();
                }
                else {
                    $('.erroremail').hide();
                    $('.successemail').show();

                }

            },
            error: function () { }
        });

    });
    $("#trainer_id").change(function () {
        $("#trainer_name").val($("#trainer_id option:selected").text());

    });



    $(window).on("load", function () {
        $(".myscroll").mCustomScrollbar({
            setHeight: 200
        });
    });



});

$("#DOB").datepicker({
    startDate: "-50y",
    format: "dd/MM/yyyy",
    endDate: Date()


});

$(".DOB").datepicker({
    startDate: "-50y",
    format: "dd/MM/yyyy",
    endDate: Date()


});



$("#certificate_date").datepicker({
    startDate: "-20y",
    format: "dd/MM/yyyy",
    endDate: Date()


});


$(".date_picker").datepicker({
    startDate: Date(),
    format: "dd/MM/yyyy"
});
$('.timepicker-24').timepicker({
    autoclose: true,
    minuteStep: 5,
    showSeconds: false,
    showMeridian: false
});
var placeholder = "الفئات المستهدفة";
$.fn.select2.defaults.set("theme", "bootstrap");
$('.select2-search__field').width("100%");
$(".select2-multiple").select2({
    placeholder: placeholder,
    width: '100%'

});
$("#s_mem_cat").change(function () {

    if ($("#s_mem_cat").val() == "true") {
        //  alert($("#s_mem_cat").val());
        if ($("#S_gender").hasClass("hidden"))
            $("#S_gender").removeClass("hidden");
    }
    else {
        if (!$("#s_gender").hasClass("hidden"));
        $("#S_gender").addClass("hidden");
    }

});

$('#connectorChb').on('ifChecked', function (event) {
    $("#connector_is_representative").addClass("hidden");
    $("#external_supervisor_is_connector").attr("disabled", "disabled");

});
$('#connectorChb').on('ifUnchecked', function (event) {
    $("#connector_is_representative").removeClass("hidden");
    $("#external_supervisor_is_connector").removeAttr("disabled");
});



$('#external_supervisor_is_connector').on('ifChecked', function (event) {
    $("#external_supervisor").addClass("hidden");


});
$('#external_supervisor_is_connector').on('ifUnchecked', function (event) {
    $("#external_supervisor").removeClass("hidden");

});



$('#external_supervisor_is_representative').on('ifChecked', function (event) {
    $("#external_supervisor").addClass("hidden");

});
$('#external_supervisor_is_representative').on('ifUnchecked', function (event) {
    $("#external_supervisor").removeClass("hidden");

});

//$('.radioCheck').on('change', function (event) {
//    $("#member_category-error").remove();

//    if ($(this).val() == "true") {
//        if ($("#single").hasClass("hidden")) {
//            $("#single").removeClass("hidden");
//        }
//        if (!$("#organization").hasClass("hidden")) {
//            $("#organization").addClass("hidden");
//        }

//    }
//    else {

//        if (!$("#single").hasClass("hidden")) {
//            $("#single").addClass("hidden");
//        }
//        if ($("#organization").hasClass("hidden")) {
//            $("#organization").removeClass("hidden");
//        }

//    }


//});

$('#iswork').on('ifChecked', function (event) {
    // var $validator = $("#admin_form").validate();
    // $("input[name*=certified_trainer]").rules("add", "required");
    $("#Iwork").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#Iemployer").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    /* Build up errors object, name of input and error message: */
    //errors = { rules: { job: required }, messages: { job: "هذا الحقل اجباري" } };
    ///* Show errors on the form */
    //$validator.showErrors(errors);   
});
$('#single_member').on('ifChecked', function (event) {
    if ($("#single").hasClass("hidden")) {
        $("#single").removeClass("hidden");
    }
    if (!$("#organization").hasClass("hidden")) {
        $("#organization").addClass("hidden");
    }
    $("#fullname_single").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#national_number").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#gender").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#member_type_single").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });


});
$('#single_member').on('ifUnchecked', function (event) {
    if (!$("#single").hasClass("hidden")) {
        $("#single").addClass("hidden");
    }
    $("#fullname_single").rules("remove");
    $("#national_number").rules("remove");
    $("#gender").rules("remove");
    $("#member_type_single").rules("remove");
});
$('#group_member').on('ifChecked', function (event) {

    if (!$("#single").hasClass("hidden")) {
        $("#single").addClass("hidden");
    }
    if ($("#organization").hasClass("hidden")) {
        $("#organization").removeClass("hidden");
    }
    $("#fullname_org").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#orgnization_number").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#responsible_person").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#responsible_person_job").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#member_type_org").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
});
$('#group_member').on('ifUnchecked', function (event) {
    if (!$("#organization").hasClass("hidden")) {
        $("#organization").addClass("hidden");
    }
    $("#fullname_org").rules("remove");
    $("#orgnization_number").rules("remove");
    $("#responsible_person").rules("remove");
    $("#responsible_person_job").rules("remove");
    $("#member_type_org").rules("remove");
});


$('#certified_trainer').on('ifChecked', function (event) {

    $("#certificate_date").removeAttr("disabled");
    $("#certificate_name").removeAttr("disabled");
    $("#certificate_date").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });
    $("#certificate_name").rules("add", {
        required: true,
        messages: {
            required: "هذا الحقل اجباري"
        }
    });

});
$('#certified_trainer').on('ifUnchecked', function (event) {


    $("#certificate_date").rules("remove");
    $("#certificate_name").rules("remove");
    $("#certificate_date").attr("disabled", "disabled");
    $("#certificate_name").attr("disabled", "disabled");

});

$('#iswork').on('ifUnchecked', function (event) {
    $("#Iwork").rules("remove");
    $("#Iemployer").rules("remove");
});


$('#uniform').on('ifChecked', function (event) {
    $("#uniform_details").attr("name", "uniform_details");
});
$('#uniform').on('ifUnchecked', function (event) {
    $("#uniform_details").removeAttr("name");
    $("#uniform_details").rules('remove');
});

$('#skill').on('ifChecked', function (event) {
    $("#skill_details").attr("name", "skill_details");

});
$('#skill').on('ifUnchecked', function (event) {
    $("#skill_details").removeAttr("name");
    $("#skill_details").rules('remove');
});

$('#train').on('ifChecked', function (event) {
    $("#train_details").attr("name", "train_details");
});
$('#train').on('ifUnchecked', function (event) {
    $("#train_details").removeAttr("name");
    $("#train_details").rules('remove');
});


$('#allow_rating').on('ifChecked', function (event) {
    $("#start_rating").attr("name", "start_rating");
    $("#end_rating").attr("name", "end_rating");
    $("#start_rating").removeAttr("disabled");
    $("#end_rating").removeAttr("disabled");
});
$('#allow_rating').on('ifUnchecked', function (event) {
    $("#start_rating").removeAttr("name");
    $("#end_rating").removeAttr("name");
    $("#start_rating").attr("disabled", "disabled");
    $("#end_rating").attr("disabled", "disabled");


});


var imageCropWidth = 0;
var imageCropHeight = 0;
var cropPointX = 0;
var cropPointY = 0;


initCrop();


$("#hl-crop-image").on("click", function (e) {
    e.preventDefault();
    if ($("#uploadImage").val() == "" || $("#uploadImage").val() == null) {
        alert("يرجى اختيار صورة مناسبة");
        return false;
    }

    cropImage();
});

function initCrop() {
    $('#my-origin-image').Jcrop({
        onChange: setCoordsAndImgSize,
        setSelect: [0, 0, 400, 400],
        aspectRatio: 1,
        minSize: [400, 400],
        maxSize: [500, 500],
        bgFade: true, // use fade effect
        bgOpacity: .3 // fade opacity

    });
}

function setCoordsAndImgSize(e) {

    imageCropWidth = e.w;
    imageCropHeight = e.h;

    cropPointX = e.x;
    cropPointY = e.y;
}

function cropImage() {

    if (imageCropWidth == 0 && imageCropHeight == 0) {
        alert("Please select crop area.");
        return;
    }

    $.ajax({
        url: '/ajax/CropImage',
        type: 'POST',
        data: {
            imagePath: $("#my-origin-image").attr("src"),
            cropPointX: cropPointX,
            cropPointY: cropPointY,
            imageCropWidth: imageCropWidth,
            imageCropHeight: imageCropHeight
        },
        success: function (data) {
            // $('.jcrop-holder img').attr("src", data.photoPath + "?t=" + new Date().getTime());
            // initCrop();
            console.log(data);
            $("#my-cropped-image")
                .attr("src", data.photoPath + "?t=" + new Date().getTime())
                .show();
        },
        error: function (data) { }
    });
}
$("#uploadImage").on("change", function () {

    var src = $("#uploadImage");
    var target = $("#my-origin-image");
    readUrl(target, src);

});
function readUrl(target, src) {
    // alert(src.get(0).files[0].name);
    console.log(src.get(0).files);

    if (src.get(0).files && src.get(0).files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            target.attr('src', e.target.result);
            //jcrop_api.setImage(e.target.result);
            var image = new Image();
            image.src = e.target.result;
            target.removeAttr('width');
            target.removeAttr('height');
            target.attr('width', image.width + 'px');
            target.attr('height', image.height + 'px');
            $('.jcrop-holder img').attr('src', e.target.result);
            $('.jcrop-holder img').removeAttr('width');
            $('.jcrop-holder img').removeAttr('height');
            $('.jcrop-holder img').attr('width', image.width + 'px');
            $('.jcrop-holder img').attr('height', image.height + 'px');

        };
        reader.readAsDataURL(src.get(0).files[0]);
        initCrop(target);

    }
    else alert(src.get(0).files);
}

//////////////////////////Map ///////////

var marker;
var map;
function initialize() {
    var mapOptions = {
        zoom: 13,
        center: { lat: 24.6877300, lng: 46.7218500 }
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
            mapOptions);

    marker = new google.maps.Marker({
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: { lat: 24.6877300, lng: 46.7218500 }
    });
    google.maps.event.addListener(map, 'click', function (a) {
        //toggleBounce();
        $("#longitude").val(a.latLng.lng().toFixed(6));
        $("#latitude").val(a.latLng.lat().toFixed(6));
        var latlng = new google.maps.LatLng(a.latLng.lat().toFixed(6), a.latLng.lng().toFixed(6));
        marker.setPosition(latlng);
    });
    google.maps.event.addListener(marker, 'dragend', function (a) {
        $("#longitude").val(a.latLng.lng().toFixed(6));
        $("#latitude").val(a.latLng.lat().toFixed(6));
    });
    var input = document.getElementById('pac-input');

    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);


    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers = [];
    // [START region_getplaces]
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        //markers.forEach(function (marker) {
        //    marker.setMap(null);
        //});
        //markers = [];

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            // Create a marker for each place.
            //markers.push(new google.maps.Marker({
            //    map: map,
            //    icon: icon,
            //    title: place.name,
            //    position: place.geometry.location
            //}));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}
function toggleBounce() {

    if (marker.getAnimation() != null) {
        marker.setAnimation(null);
    } else {
        marker.setAnimation(google.maps.Animation.BOUNCE);
    }
}

//File Upload response from the server
Dropzone.options.dropzoneForm = {
    maxFiles: 2,
    init: function () {
        this.on("maxfilesexceeded", function (data) {
            var res = eval('(' + data.xhr.responseText + ')');

        });
        this.on("addedfile", function (file) {

            // Create the remove button
            var removeButton = Dropzone.createElement("<button>Remove file</button>");


            // Capture the Dropzone instance as closure.
            var _this = this;

            // Listen to the click event
            removeButton.addEventListener("click", function (e) {
                // Make sure the button click doesn't submit the form:
                e.preventDefault();
                e.stopPropagation();
                // Remove the file preview.
                _this.removeFile(file);
                // If you want to the delete the file on the server as well,
                // you can do the AJAX request here.
            });

            // Add the button to the file preview element.
            file.previewElement.appendChild(removeButton);
        });
    }
};

///////////////// modal Sections //////////////

var ele = $(".accept_member");
var btn = $(".rejectBtn");
var btn_memeber = $(".rejectBtn_member");
var accept_btn = $(".accept_btn");
if (ele != null) {
    $(".accept_member").click(function () {
        var id = $(this).attr("data-id");
        var name = $(this).attr("data-name");
        $("#member_id").val(id);
        $("#member_name").val(name);

    });
}

if (btn != null) {
    $(".rejectBtn").click(function () {
        var id = $(this).attr("data-id");
        var name = $(this).attr("data-name");
        $("#volunteer_id").val(id);
        $("#volunteer_name").val(name);

    });

    $(".hBtn").click(function () {
        var id = $(this).attr("data-id");
        var name = $(this).attr("data-name");
        console.log(name);
        console.log(id);
        $("#hvolunteer_id").val(id);
        $("#hvolunteer_name").val(name);

    });

    if (btn_memeber != null) {
        $(".rejectBtn_member").click(function () {
            var id = $(this).attr("data-id");
            var name = $(this).attr("data-name");
            $("#member_id_r").val(id);
            $("#member_name_r").val(name);

        });
    }
    if (accept_btn != null) {
        accept_btn.click(function () {
            var id = $(this).attr("data-id");
            var name = $(this).attr("data-name");
            $("#opp_id").val(id);
            $("#opp_name").val(name);

        });

    }
}
$('#mymdl').on('shown.bs.modal', function () {
    $('.money').inputmask("decimal", {
        radixPoint: ".",
        groupSeparator: ",",
        digits: 2,
        autoGroup: true,
        //  prefix: 'SR', //No Space, this will truncate the first character
        rightAlign: false,
        oncleared: function () { self.Value(''); }
    });

});

///////////////////////////////////////////////
$("#addBtn").click(function () {
    var item = '';

    item = '<fieldset>' +
                                          '<legend>عضو جديد:</legend>' +
                                           '<div class="row">' +
                                       '<div class="col-md-4">' +
                                                  ' <div class="form-group">' +
                                                       '<div class="input-group">' +
                                                          ' <span class="input-group-addon">' +

                                                            '<i class="fa fa-user"></i>' +
                                                           '</span>' +
                                                          ' <input type="text" class="form-control" name="member[' + index + '].fullname" placeholder="اسم المجموعة ....">' +
                                                       '</div>' +
                                                  ' </div>' +

                                              ' </div>' +
                                               '<div class="col-md-4">' +
                                                  ' <div class="form-group">' +
                                                      ' <div class="input-group">' +
                                                          ' <span class="input-group-addon">' +
                                                              ' <i class="fa  fa-globe"></i>' +
                                                         '  </span>' +
                                                           '<input type="text" class="form-control" name="member[' + index + '].job" placeholder="المسمى الوظيفي ....">' +
                                                       '</div>' +
                                                   '</div>' +
    '</div>' +
    '<div class="col-md-4">' +
        '<div class="form-group">' +
            '<div class="input-group">' +
                '<span class="input-group-addon">' +
                    '<i class="fa fa-flag" aria-hidden="true"></i>' +
                '</span>' +
               ' <input type="text" class="form-control" name="member[' + index + '].degree" placeholder="الدرجة العلمية">' +
            '</div>' +
       '</div>' +
    '</div>' +
    '</div>' +
    '<div class="row">' +
    '<div class="col-md-4">' +
        '<div class="form-group">' +
           '<div class="input-group">' +
                '<span class="input-group-addon">' +
                    '<i class="fa fa-building"></i>' +
                '</span>' +
                '<input type="text" class="form-control" name="member[' + index + '].position_in_group" placeholder="الصفة في المجموعة ....">' +
            '</div>' +
        '</div>' +
    '</div>' +
    '<div class="col-md-4">' +
        '<div class="form-group">' +
            '<div class="input-group">' +
               '<span class="input-group-addon">' +
                    '<i class="fa fa-code-fork"></i>' +
                '</span>' +
                '<input type="text" class="form-control" name="member[' + index + '].national_id" placeholder="رقم السجل المدني">' +
            '</div>' +
    '</div>' +
    '</div>' +
    '<div class="col-md-4">' +
        ' <div class="form-group">' +
             '<div class="input-group">' +
                ' <span class="input-group-addon">' +

                    ' <i class="fa fa-user"></i>' +
    '</span>' +
    '<input type="text" class="form-control DOB" name="member[' + index + '].DOB"  placeholder="تاريخ الميلاد ....">' +
    '</div>' +
    '</div>' +

    '</div>' +
    //' <div class="col-md-4">' +
    //     '<div class="form-group">' +
    //         '<div class="input-group">' +
    //             '<span class="input-group-addon">' +
    //                 '<i class="fa fa-file-image-o" aria-hidden="true"></i>' +
    //            ' </span>' +
    //            ' <input type="text" class="form-control" name="member[' + index + '].nationality" placeholder="الجنسية ">' +

    //         '</div>' +
    //     '</div>' +

    // '</div>' +
    '</div>' +
    ' <div class="row">' +

    '<div class="col-md-4">' +
        '<div class="form-group">' +
            '<div class="input-group">' +
               ' <span class="input-group-addon">' +
                    '<i class="fa  fa-globe"></i>' +
    '</span>' +

    '<input type="text" class="form-control" name="member[' + index + '].mobile" placeholder="رقم الجوال ....">' +

    '</div>' +
    '</div>' +
    '</div>' +
    '<div class="col-md-4">' +
        '<div class="form-group">' +

            '<div class="input-group">' +
                '<span class="input-group-addon">' +
                    '<i class="fa fa-flag" aria-hidden="true"></i>' +
    '</span>' +
    '<input type="text" class="form-control" name="member[' + index + '].email" placeholder="البريد الالكتروني">' +
    '</div>' +
    '</div>' +

    '</div>' +
    '</div>' +
    '</fieldset>';
    $(".newMem").append(item);
    index = index + 1;
});

$(".configration form").keypress(function () {

    return event.keyCode != 13;
});
var pageIndex = 0;
//////////////////////////////////////////ajax////////////

$(window).scroll(function () {
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        // if ($(".pagenum:last").val() <= $(".rowcount").val()) {
        pageIndex = pageIndex + 1;
        getresult('/ajax/getArchive?pageIndex=' + pageIndex);
        // }
    }
});
function getresult(url) {
    $.ajax({
        url: url,
        type: "GET",
        beforeSend: function () {
            $('#loader-icon').show();
        },
        complete: function () {
            $('#loader-icon').hide();
        },
        success: function (data) {
            $(".feeds").append(data);
            //   console.log(data);
        },
        error: function () { }
    });
}

function getPending() {
    $.ajax({
        url: '/ajax/_GetList/',
        type: "GET",
        //beforeSend: function () {
        //    $('#loader-icon').show();
        //},
        //complete: function () {
        //    $('#loader-icon').hide();
        //},
        success: function (data) {
            $(".topbar-actions ul.myList").html(data);
            console.log(data.length);
        },
        error: function () { }
    });
}


$(".viewDetails").click(function () {
    var id = $(this).attr("data-id");

    // $("#userInformation").on('show.bs.modal', function () {

    $.ajax({
        url: '/ajax/GetUserInfo/' + id,
        type: "GET",
        beforeSend: function () {
            $('#loader-icon').show();
        },
        complete: function () {
            $('#loader-icon').hide();
        },
        success: function (data) {
            $(".renderInfo").html(data);
            //   console.log(data);
        },
        error: function () { }
    });
    // });

});


function validateEmail(email) {

    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    return pattern.test(email);
}