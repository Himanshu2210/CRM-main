
//$(".body-content").click(function () {
//    $(".sidenav").css("width", "0");
//});
$(".User01").click(function () {
    $(".Profile-wrp").fadeToggle("");
});


// sidebar
function openNav() {
    document.getElementById("mySidenav").style.width = "270px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

// slide end------
// datepicker

document.getElementById('datePicker').valueAsDate = new Date();



// validatio start here


function validateFn() {
    var name = document.f1.name.value;
    var ordernumber = document.f1.ordernumber.value;
    var createdate = document.f1.createdate.value;
    // var passwordlength=document.f1.password.value.length;  
    var status = false;
    if (name == "") {
        var clr = document.getElementById("UserName");
        clr.value = "* Please enter your name";
        clr.style.color = "red";
        status = false;
    }
    else {
        document.getElementById("UserName").innerHTML = " <img src=''/>";
        status = true;
    }

    //order number
    if (ordernumber == "") {
        var ord = document.getElementById("OrderNumber");
        ord.value = " Please enter your Valid order number";
        ord.style.color = "red";
        status = false;
    }
    else {
        document.getElementById("ordernumber").innerHTML = " <img src=''/>";
        status = true;
    }

    // if(createdate==""){  
    // document.getElementById("createdate").innerHTML=  
    // " Please Selecte Date";  
    // status=false;
    // }else{  
    // document.getElementById("createdate").innerHTML=" <img src=''/>";  
    // status=true;
    // }       




    // if(passwordlength<6){  
    // document.getElementById("passwordlocation").innerHTML=  
    // "Password must be greater than 6";  
    // status=false; 
    // }else{  
    // document.getElementById("passwordlocation").innerHTML=" <img src=''/>";  
    // }  
    var email = document.f1.email.value;
    var phonenumber = document.f1.phonenumber.value;
    var skype = document.f1.skype.value;
    var whatsnumber = document.f1.whatsnumber.value;
    if (email === '' || phonenumber === '' || skype === '' || whatsnumber === '') {

        var scm = document.getElementById("mandatory");
        scm.innerHTML =
            "One of the following is mandatory :Email id, Skype id , Phone number, whatsApp phone number ";
        scm.style.display = "block";
        status = false;
    } else {
        document.getElementById("mandatory").innerHTML = " <img src=''/>";
        status = true;
    }
    return status;
}

// validation end here--========--


// view page popup
//function myPopup() {
//    var x = document.getElementById("main01");
//    if (x.style.display === "block") {
//        x.style.display = "none";
//    } else {
//        x.style.display = "block";
//    }
//}

//  veiw pop up end here
// //// submit pop up 

function mSubmit() {
    var x = document.getElementById("main02");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }
}



//  veiw pop up end here
// //// submit pop up 2

function mSubmit2() {
    var y = document.getElementById("main01");
    var x = document.getElementById("main02");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
        y.style.display = "none";
    }
}

// //// delete1 pop up 3

function mDelete() {
    // var y = document.getElementById("main01");
    var x = document.getElementById("main03");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
        //   y.style.display = "none";
    }
}


//  veiw pop up end here
// //// delete2 pop up 4

function mDelete2() {
    var y = document.getElementById("main03");
    var x = document.getElementById("modeldelete");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
        y.style.display = "none";
    }
}

//record actived pop up
function recordFn() {
    var x = document.getElementById("main04");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }
}

    //    peritcles js
