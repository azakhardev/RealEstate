let pictures = document.querySelectorAll('.photoSmall img');
let picturesDiv = document.querySelectorAll('.photoSmall');
let mainSmallPicture = document.querySelector('.mainPhotoSmall');
let i = 0;
picturesDiv.forEach(p =>
{
    if (i > 4)
    {
        p.style.display = "none";
    }
    i++;
})

pictures.forEach(clickedPicture => {
    clickedPicture.addEventListener('click', () => {
        const mainPicture = document.querySelector('.photoBig img');
        mainPicture.setAttribute('src', clickedPicture.src);
    });
});

let number = 5;
let next = document.querySelector('div .nextPhotos')
let prev = document.querySelector('div .previousPhotos')

next.addEventListener('click', () => {          
    if (number + 1 < pictures.length + 1) {
        picturesDiv[number - 5].style.display = "none";
        picturesDiv[number].style.display = "block";
        number += 1;
    }
})

prev.addEventListener('click', () => {

    if (number - 1 >= 5) {
        number -= 1;
        picturesDiv[number].style.display = "none";
        picturesDiv[number - 5].style.display = "block";                   
    }
})