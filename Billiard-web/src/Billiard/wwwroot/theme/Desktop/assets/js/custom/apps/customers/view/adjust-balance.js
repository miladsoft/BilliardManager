"use strict";var KTModalAdjustBalance=function(){var t,e,n,o,a,r,i,l,c;return{init:function(){t=document.querySelector("#ppc_modal_adjust_balance"),c=new bootstrap.Modal(t),l=t.querySelector("#ppc_modal_adjust_balance_form"),e=l.querySelector("#ppc_modal_adjust_balance_submit"),n=l.querySelector("#ppc_modal_adjust_balance_cancel"),o=t.querySelector("#ppc_modal_adjust_balance_close"),Inputmask("US$ 9,999,999.99",{numericInput:!0}).mask("#ppc_modal_inputmask"),function(){const e=t.querySelector('[ppc-modal-adjust-balance="current_balance"]');i=t.querySelector('[ppc-modal-adjust-balance="new_balance"]'),r=document.getElementById("ppc_modal_inputmask");let n,o=parseFloat(e.innerHTML.replace(/[^0-9.]/g,"").replace(",",""));r.addEventListener("focusout",(function(t){n=parseFloat(t.target.value.replace(/[^0-9.]/g,"").replace(",","")),isNaN(n)&&(n=0),i.innerHTML="US$ "+(n+o).toFixed(2).replace(/\d(?=(\d{3})+\.)/g,"$&,")}))}(),a=FormValidation.formValidation(l,{fields:{adjustment:{validators:{notEmpty:{message:"Adjustment type is required"}}},amount:{validators:{notEmpty:{message:"Amount is required"}}}},plugins:{trigger:new FormValidation.plugins.Trigger,bootstrap:new FormValidation.plugins.Bootstrap5({rowSelector:".fv-row",eleInvalidClass:"",eleValidClass:""})}}),$(l.querySelector('[name="adjustment"]')).on("change",(function(){a.revalidateField("adjustment")})),e.addEventListener("click",(function(t){t.preventDefault(),a&&a.validate().then((function(t){console.log("validated!"),"Valid"==t?(e.setAttribute("data-ppc-indicator","on"),e.disabled=!0,setTimeout((function(){e.removeAttribute("data-ppc-indicator"),Swal.fire({text:"فرم با موفقیت ارسال شد!",icon:"success",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn btn-primary"}}).then((function(t){t.isConfirmed&&(c.hide(),e.disabled=!1,l.reset(),i.innerHTML="--")}))}),2e3)):Swal.fire({text:"متأسفیم ، به نظر می رسد برخی خطاها شناسایی شده است ، لطفاً دوباره امتحان کنید.",icon:"error",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn btn-primary"}})}))})),n.addEventListener("click",(function(t){t.preventDefault(),Swal.fire({text:"آیا مطمئن هستید که می خواهید لغو کنید؟?",icon:"warning",showCancelButton:!0,buttonsStyling:!1,confirmButtonText:"بله ، آن را لغو کنید!",cancelButtonText:"خیر",customClass:{confirmButton:"btn btn-primary",cancelButton:"btn btn-active-light"}}).then((function(t){t.value?(l.reset(),c.hide()):"cancel"===t.dismiss&&Swal.fire({text:"فرم شما لغو نشده است !.",icon:"error",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn btn-primary"}})}))})),o.addEventListener("click",(function(t){t.preventDefault(),Swal.fire({text:"آیا مطمئن هستید که می خواهید لغو کنید؟?",icon:"warning",showCancelButton:!0,buttonsStyling:!1,confirmButtonText:"بله ، آن را لغو کنید!",cancelButtonText:"خیر",customClass:{confirmButton:"btn btn-primary",cancelButton:"btn btn-active-light"}}).then((function(t){t.value?(l.reset(),c.hide()):"cancel"===t.dismiss&&Swal.fire({text:"فرم شما لغو نشده است !.",icon:"error",buttonsStyling:!1,confirmButtonText:"باشه فهمیدم!",customClass:{confirmButton:"btn btn-primary"}})}))}))}}}();PPCUtil.onDOMContentLoaded((function(){KTModalAdjustBalance.init()}));