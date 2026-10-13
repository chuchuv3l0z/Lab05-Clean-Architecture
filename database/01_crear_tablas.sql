-- Ejecuta en una base vacía de PostgreSQL. No elimina tablas ni datos.
-- El modelo académico original usa lab05; usuarios usa public.
CREATE SCHEMA IF NOT EXISTS lab05;
CREATE TABLE IF NOT EXISTS lab05.estudiantes (
 id_estudiante SERIAL PRIMARY KEY, nombre VARCHAR(100) NOT NULL, edad INTEGER NOT NULL,
 direccion VARCHAR(255), telefono VARCHAR(20), correo VARCHAR(100));
CREATE TABLE IF NOT EXISTS lab05.cursos (
 id_curso SERIAL PRIMARY KEY, nombre VARCHAR(100) NOT NULL, descripcion TEXT, creditos INTEGER NOT NULL);
CREATE TABLE IF NOT EXISTS lab05.profesores (
 id_profesor SERIAL PRIMARY KEY, nombre VARCHAR(100) NOT NULL, especialidad VARCHAR(100), correo VARCHAR(100));
CREATE TABLE IF NOT EXISTS lab05.materias (
 id_materia SERIAL PRIMARY KEY, nombre VARCHAR(100) NOT NULL, descripcion TEXT,
 id_curso INTEGER REFERENCES lab05.cursos(id_curso));
CREATE TABLE IF NOT EXISTS lab05.matriculas (
 id_matricula SERIAL PRIMARY KEY, id_estudiante INTEGER REFERENCES lab05.estudiantes(id_estudiante),
 id_curso INTEGER REFERENCES lab05.cursos(id_curso), semestre VARCHAR(20));
CREATE TABLE IF NOT EXISTS lab05.asistencias (
 id_asistencia SERIAL PRIMARY KEY, id_estudiante INTEGER REFERENCES lab05.estudiantes(id_estudiante),
 id_curso INTEGER REFERENCES lab05.cursos(id_curso), fecha DATE, estado VARCHAR(20));
CREATE TABLE IF NOT EXISTS lab05.evaluaciones (
 id_evaluacion SERIAL PRIMARY KEY, id_estudiante INTEGER REFERENCES lab05.estudiantes(id_estudiante),
 id_curso INTEGER REFERENCES lab05.cursos(id_curso), calificacion NUMERIC(5,2), fecha DATE);
CREATE TABLE IF NOT EXISTS public.usuarios (
 id_usuario SERIAL PRIMARY KEY, username VARCHAR(100) NOT NULL,
 password_hash VARCHAR(255) NOT NULL, rol VARCHAR(20) NOT NULL);
CREATE UNIQUE INDEX IF NOT EXISTS ux_usuarios_username ON public.usuarios(username);
CREATE UNIQUE INDEX IF NOT EXISTS ux_matriculas_estudiante_curso_semestre
 ON lab05.matriculas(id_estudiante, id_curso, semestre);
