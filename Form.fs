namespace Central

module Form =
  let focus (form: Eto.Forms.Form) =
    form.Visible <- true
    form.BringToFront()
