namespace HelloDocMVC.wwwroot.js
{
    public class darkTheme
    {
    }
}


function darkTheme()
{
        event.preventDefault();
    let element = document.body;
    let elementnav = document.getElementById("NavBar")

        let inputs = document.getElementsByTagName("input")

        let textarea = document.getElementsByTagName("textarea")
    if (localStorage.getItem('mode'))
    {
        localStorage.removeItem('mode')
                console.log("mode patient: ", localStorage.getItem("mode"));
        element.classList.remove("dark");
        elementnav.classList.remove("navdark")
        //for (let i = 0; i < inputs.length; i++) {
        //    inputs[i].classList.remove("inputcolor")

        //}
        //for (let i = 0; i < textarea.length; i++) {
        //    textarea[i].classList.remove("inputcolor")

        //}

    console.log("mode  req: ", localStorage.getItem("mode"));
    }
    else
    {
        localStorage.setItem("mode", "dark")
                console.log("mode patient: ", localStorage.getItem("mode"));
        element.classList.add("dark")
        elementnav.classList.add("navdark")
        //for (let i = 0; i < inputs.length; i++) {
        //    inputs[i].classList.add("inputcolor")

        //}
        //for (let i = 0; i < textarea.length; i++) {
        //    textarea[i].classList.add("inputcolor")

        //}
    console.log("mode  req: ", localStorage.getItem("mode"));
    }

}
