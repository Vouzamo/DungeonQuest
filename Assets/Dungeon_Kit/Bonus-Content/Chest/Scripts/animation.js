

function OnMouseEnter () {
    animation.Play("open");
    yield WaitForSeconds (1.75);
    animation.Play("close");
}
