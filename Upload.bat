git rm -r --cached .
echo .vs/ > .gitignore
echo publish/ >> .gitignore
echo bin/ >> .gitignore
echo obj/ >> .gitignore

git add .
git commit -m "Initial commit with Dockerfile"
git branch -M main
git remote add origin https://github.com/13vidocq13/StatML.git
git push -u origin main
pause