import { useState, useEffect } from 'react';
import { ArrowLeft, BookOpen, Clock, CheckCircle, Play } from 'lucide-react';
import { supabase, Course, Lesson, Enrollment } from '../lib/supabase';
import { useAuth } from '../contexts/AuthContext';

type CourseDetailViewProps = {
  courseId: string;
  onBack: () => void;
};

type CourseWithInstructor = Course & {
  instructor_name: string;
};

export const CourseDetailView = ({ courseId, onBack }: CourseDetailViewProps) => {
  const [course, setCourse] = useState<CourseWithInstructor | null>(null);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [enrollment, setEnrollment] = useState<Enrollment | null>(null);
  const [completedLessons, setCompletedLessons] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(true);
  const [enrolling, setEnrolling] = useState(false);
  const { user } = useAuth();

  useEffect(() => {
    loadCourseDetails();
  }, [courseId, user]);

  const loadCourseDetails = async () => {
    try {
      setLoading(true);

      const { data: courseData, error: courseError } = await supabase
        .from('courses')
        .select(`
          *,
          profiles:instructor_id (full_name)
        `)
        .eq('id', courseId)
        .single();

      if (courseError) throw courseError;

      setCourse({
        ...courseData,
        instructor_name: courseData.profiles?.full_name || 'Instrutor',
      });

      const { data: lessonsData, error: lessonsError } = await supabase
        .from('lessons')
        .select('*')
        .eq('course_id', courseId)
        .order('order_index', { ascending: true });

      if (lessonsError) throw lessonsError;
      setLessons(lessonsData || []);

      if (user) {
        const { data: enrollmentData } = await supabase
          .from('enrollments')
          .select('*')
          .eq('student_id', user.id)
          .eq('course_id', courseId)
          .maybeSingle();

        if (enrollmentData) {
          setEnrollment(enrollmentData);

          const { data: progressData } = await supabase
            .from('lesson_progress')
            .select('lesson_id')
            .eq('enrollment_id', enrollmentData.id)
            .eq('completed', true);

          setCompletedLessons(new Set((progressData || []).map(p => p.lesson_id)));
        }
      }
    } catch (error) {
      console.error('Error loading course details:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleEnroll = async () => {
    if (!user || !course) return;

    try {
      setEnrolling(true);

      const { error } = await supabase
        .from('enrollments')
        .insert({
          student_id: user.id,
          course_id: course.id,
        });

      if (error) throw error;

      await loadCourseDetails();
    } catch (error) {
      console.error('Error enrolling:', error);
    } finally {
      setEnrolling(false);
    }
  };

  const handleToggleLessonComplete = async (lessonId: string) => {
    if (!enrollment) return;

    try {
      const isCompleted = completedLessons.has(lessonId);

      if (isCompleted) {
        await supabase
          .from('lesson_progress')
          .delete()
          .eq('enrollment_id', enrollment.id)
          .eq('lesson_id', lessonId);

        setCompletedLessons(prev => {
          const newSet = new Set(prev);
          newSet.delete(lessonId);
          return newSet;
        });
      } else {
        await supabase
          .from('lesson_progress')
          .insert({
            enrollment_id: enrollment.id,
            lesson_id: lessonId,
            completed: true,
            completed_at: new Date().toISOString(),
          });

        setCompletedLessons(prev => new Set([...prev, lessonId]));
      }

      const newProgress = Math.round((completedLessons.size / lessons.length) * 100);
      await supabase
        .from('enrollments')
        .update({ progress: newProgress })
        .eq('id', enrollment.id);
    } catch (error) {
      console.error('Error updating lesson progress:', error);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!course) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-600">Curso não encontrado</p>
        <button onClick={onBack} className="mt-4 text-blue-600 hover:text-blue-700">
          Voltar
        </button>
      </div>
    );
  }

  const levelLabels = {
    beginner: 'Iniciante',
    intermediate: 'Intermediário',
    advanced: 'Avançado',
  };

  const totalDuration = lessons.reduce((sum, lesson) => sum + (lesson.duration_minutes || 0), 0);

  return (
    <div className="space-y-6">
      <button
        onClick={onBack}
        className="flex items-center space-x-2 text-gray-600 hover:text-gray-900 transition"
      >
        <ArrowLeft className="w-5 h-5" />
        <span>Voltar</span>
      </button>

      <div className="bg-white rounded-xl shadow-sm overflow-hidden">
        <div className="h-64 bg-gradient-to-br from-blue-500 to-blue-600 flex items-center justify-center">
          {course.thumbnail_url ? (
            <img src={course.thumbnail_url} alt={course.title} className="w-full h-full object-cover" />
          ) : (
            <BookOpen className="w-24 h-24 text-white opacity-50" />
          )}
        </div>

        <div className="p-8">
          <div className="flex flex-wrap items-center gap-3 mb-4">
            <span className="px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm font-medium">
              {course.category}
            </span>
            <span className="px-3 py-1 bg-slate-100 text-slate-700 rounded-full text-sm font-medium">
              {levelLabels[course.level]}
            </span>
            <span className="flex items-center text-gray-600 text-sm">
              <BookOpen className="w-4 h-4 mr-1" />
              {lessons.length} aulas
            </span>
            {totalDuration > 0 && (
              <span className="flex items-center text-gray-600 text-sm">
                <Clock className="w-4 h-4 mr-1" />
                {totalDuration} minutos
              </span>
            )}
          </div>

          <h1 className="text-4xl font-bold text-gray-900 mb-4">{course.title}</h1>

          <p className="text-lg text-gray-600 mb-6">{course.description}</p>

          <div className="flex items-center justify-between border-t border-gray-200 pt-6">
            <div className="flex items-center space-x-3">
              <div className="w-12 h-12 bg-gradient-to-br from-slate-200 to-slate-300 rounded-full flex items-center justify-center">
                <span className="text-lg font-bold text-slate-600">
                  {course.instructor_name.charAt(0).toUpperCase()}
                </span>
              </div>
              <div>
                <p className="text-sm text-gray-600">Instrutor</p>
                <p className="font-semibold text-gray-900">{course.instructor_name}</p>
              </div>
            </div>

            {!enrollment ? (
              <button
                onClick={handleEnroll}
                disabled={enrolling}
                className="px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition disabled:opacity-50"
              >
                {enrolling ? 'Inscrevendo...' : 'Inscrever-se'}
              </button>
            ) : (
              <div className="text-right">
                <p className="text-sm text-gray-600 mb-1">Seu progresso</p>
                <p className="text-2xl font-bold text-blue-600">{enrollment.progress}%</p>
              </div>
            )}
          </div>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm p-8">
        <h2 className="text-2xl font-bold text-gray-900 mb-6">Conteúdo do Curso</h2>

        <div className="space-y-3">
          {lessons.map((lesson, index) => {
            const isCompleted = completedLessons.has(lesson.id);

            return (
              <div
                key={lesson.id}
                className={`flex items-center justify-between p-4 rounded-lg border transition ${
                  enrollment
                    ? 'hover:border-blue-300 cursor-pointer'
                    : 'opacity-60'
                } ${isCompleted ? 'bg-green-50 border-green-200' : 'border-gray-200'}`}
                onClick={() => enrollment && handleToggleLessonComplete(lesson.id)}
              >
                <div className="flex items-center space-x-4">
                  <div className={`w-10 h-10 rounded-full flex items-center justify-center ${
                    isCompleted ? 'bg-green-500' : 'bg-slate-200'
                  }`}>
                    {isCompleted ? (
                      <CheckCircle className="w-6 h-6 text-white" />
                    ) : (
                      <span className="text-sm font-semibold text-slate-600">{index + 1}</span>
                    )}
                  </div>
                  <div>
                    <h3 className="font-semibold text-gray-900">{lesson.title}</h3>
                    {lesson.duration_minutes && (
                      <p className="text-sm text-gray-500 flex items-center mt-1">
                        <Clock className="w-3 h-3 mr-1" />
                        {lesson.duration_minutes} min
                      </p>
                    )}
                  </div>
                </div>

                {enrollment && (
                  <Play className="w-5 h-5 text-blue-600" />
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};
