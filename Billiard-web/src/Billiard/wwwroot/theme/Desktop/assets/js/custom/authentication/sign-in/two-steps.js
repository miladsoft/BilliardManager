"use strict";var KTSigninTwoSteps=function(){var t,n;return{init:function(){t=document.querySelector("#ppc_sing_in_two_steps_form"),(n=document.querySelector("#ppc_sing_in_two_steps_submit")).addEventListener("click",(function(e){e.preventDefault();var i=!0,o=[].slice.call(t.querySelectorAll('input[maxlength="1"]'));o.map((function(t){""!==t.value&&0!==t.value.length||(i=!1)})),!0===i?(n.setAttribute("data-ppc-indicator","on"),n.disabled=!0,setTimeout((function(){n.removeAttribute("data-ppc-indicator"),n.disabled=!1,Swal.fire({text:"شما با موفقیت تأیید شده اید!",icon:"success",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn btn-primary"}}).then((function(t){t.isConfirmed&&o.map((function(t){t.value=""}))}))}),1e3)):swal.fire({text:"Please enter valid securtiy code and try again.",icon:"error",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn fw-bold btn-light-primary"}}).then((function(){PPCUtil.scrollTop()}))}))}}}();PPCUtil.onDOMContentLoaded((function(){KTSigninTwoSteps.init()}));