const tools = document.querySelectorAll('.Tools a')

tools.forEach((t) => {
    t.addEventListener('click', () => {
        CloseAll()        
        t.classList.add('Current');
        console.log('clicked')
    });
});

function CloseAll()
{
    document.querySelectorAll('.Tools a').forEach((a) =>
    {
        a.classList.remove('Current');
    })
}