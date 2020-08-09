/// <reference path="bootstrap.min.js" />
var Name;
var Age;
var Gender;
var Email;
var CityId = 0;
var UploadResumePath = "";
var Education;

$(document).ready(function () {
    $("#Name").keypress(function (e) {
        var keyCode = e.keyCode || e.which;

        $("#lblError").html("");

        //Regex for Valid Characters i.e. Alphabets.
        var regex = /^[A-Za-z]+$/;

        //Validate TextBox value against the Regex.
        var isValid = regex.test(String.fromCharCode(keyCode));
        if (!isValid) {
            $("#lblError").html("Only Alphabets allowed.");
        }

        return isValid;
    });

    $("#Email").change(function (e) {
        var userinput = $("#Email").val();
        var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i

        if (!pattern.test(userinput)) {
            alert('not a valid e-mail address');
        }
    });





    $("#btnsave").click(function () {
        Name = $("#Name").val();
        Age = $("#Age").val();
        Gender = $('input[name="Gender"]:checked').val();
        Email = $("#Email").val();
        CityId = $("#CityId").val();

        var Educationn = [];
        $.each($("input[name='education']:checked"), function () {
            Educationn.push($(this).val());
        });
        Education = Educationn.join(", ");



        if (Name == "") {
            alert("Name is required.");
            return false;

        }
        if (Age == "") {
            alert("Enter your Age.");
            return false;

        }
        if (Email == "") {
            alert("Email is required.");
            return false;

        }

        if (!$('[name="Gender"]:checked').length === 1) {
            alert('Please select your gender');
            return false;
        }
        if ($("#Gender:checked").length == 0) {
            alert("Gender is required.");
            return false;
        }
        if (Education == "") {
            alert("Education is required.");
            return false;

        }
        if (CityId == 0) {
            alert("Select your City.");
            return false;

        }

        if (UploadResumePath == "") {
            alert("Upload your Resume.");
            return false;

        }



        $.ajax({
            url: '/Home/SaveSurvey',
            data: { Name: Name, Age: Age, Gender: Gender, Email: Email, Education: Education, UploadResumePath: UploadResumePath, CityId: CityId },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                if (result.flag > 0)
                    alert(result.msg);
                $("#Name").val('');
                $("#Age").val('');
                $("#CityId").val('');
                $("#Education").val('');
                $("#imageUpload").val('');
                $("#Email").val('');

                $('input[type=radio]').each(function () {
                    this.checked = false;
                });
                $('input[type=checkbox]').each(function () {
                    this.checked = false;
                });

            },
            error: function (err) {
                console.log(err.statusText);
            }
        });
    });

    $('#imageUpload').change(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#imageUpload").get(0);
            var files = fileUpload.files;

            var files = $("#imageUpload").get(0).files;
            var fileSize = files[0].size;
            var fileExt = $("#imageUpload").val().split('.').pop();
            if (fileSize > 1048576) {
                alert("Resume should not be more than 1MB.");
                return false;
            }
            if (fileExt != "txt") {
                alert("Resume file should be in text file.");
                return false;
            }

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object  
            fileData.append('username', 'Resume');

            $.ajax({
                url: '/Home/UploadFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    UploadResumePath = result.filename;
                    //alert(result.msg);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $("#btncancel").click(function () {
        $("#Name").val('');
        $("#Age").val('');
        $("#CityId").val('');
        $("#Education").val('');
        $("#imageUpload").val('');
        $("#Email").val('');

        $('input[type=radio]').each(function () {
            this.checked = false;
        });
        $('input[type=checkbox]').each(function () {
            this.checked = false;
        });
    });

});

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
