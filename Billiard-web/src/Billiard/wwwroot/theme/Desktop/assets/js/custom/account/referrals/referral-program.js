"use strict";
var KTAccountReferralsReferralProgramlink={init:function(){var e,r;e=document.querySelector("#ppc_referral_program_link_copy_btn"),r=document.querySelector("#ppc_referral_link_input"),new ClipboardJS(e).on("success",(function(s){var n=e.innerHTML;r.classList.add("bg-success"),r.classList.add("text-inverse-success"),e.innerHTML="کپی شد",setTimeout((function(){e.innerHTML=n,r.classList.remove("bg-success"),r.classList.remove("text-inverse-success")}),3e3),s.clearSelection()}))}};PPCUtil.onDOMContentLoaded((function(){KTAccountReferralsReferralProgramlink.init()}));

var KTAccountReferralsReferralProgramcode={init:function(){var ecode,rcode;ecode=document.querySelector("#ppc_referral_program_code_copy_btn"),rcode=document.querySelector("#ppc_referral_code_input"),new ClipboardJS(ecode).on("success",(function(scode){var ncode=ecode.innerHTML;rcode.classList.add("bg-success"),rcode.classList.add("text-inverse-success"),ecode.innerHTML="کپی شد",setTimeout((function(){ecode.innerHTML=ncode,rcode.classList.remove("bg-success"),rcode.classList.remove("text-inverse-success")}),3e3),scode.clearSelection()}))}};PPCUtil.onDOMContentLoaded((function(){KTAccountReferralsReferralProgramcode.init()}));