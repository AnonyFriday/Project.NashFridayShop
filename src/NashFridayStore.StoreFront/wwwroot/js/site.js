document.body.addEventListener("htmx:configRequest", function (event) {
  var token = document.querySelector('input[name="__RequestVerificationToken"]');
  if (token) {
    event.detail.headers["RequestVerificationToken"] = token.value;
  }
});
