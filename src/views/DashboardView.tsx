import { useState, useEffect } from 'react';
import { TrendingUp, BookOpen, Award, Clock } from 'lucide-react';
import { supabase, Course, Enrollment } from '../lib/supabase';
import { useAuth } from '../contexts/AuthContext';
import { CourseCard } from '../components/CourseCard';

type DashboardViewProps = {
  onViewCourseDetails: (courseId: string) => void;
};

type EnrollmentWithCourse = Enrollment & {
  course: Course & {
    instructor_name: string;
    lesson_count: number;
  };
};

export const DashboardView = ({ onViewCourseDetails }: DashboardViewProps) => {
  const [enrollments, setEnrollments] = useState<EnrollmentWithCourse[]>([]);
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();

  useEffect(() => {
    loadEnrollments();
  }, [user]);

  const loadEnrollments = async () => {
    if (!user) return;

    try {
      setLoading(true);

      const { data: enrollmentsData, error: enrollmentsError } = await supabase
        .from('enrollments')
        .select(`
          *,
          courses:course_id (
            *,
            profiles:instructor_id (full_name)
          )
        `)
        .eq('student_id', user.id)
        .order('enrolled_at', { ascending: false });

      if (enrollmentsError) throw enrollmentsError;

      const enrollmentsWithDetails = await Promise.all(
        (enrollmentsData || []).map(async (enrollment: any) => {
          const { count: lessonCount } = await supabase
            .from('lessons')
            .select('*', { count: 'exact', head: true })
            .eq('course_id', enrollment.course_id);

          return {
            ...enrollment,
            course: {
              ...enrollment.courses,
              instructor_name: enrollment.courses.profiles?.full_name || 'Instrutor',
              lesson_count: lessonCount || 0,
            },
          };
        })
      );

      setEnrollments(enrollmentsWithDetails);
    } catch (error) {
      console.error('Error loading enrollments:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  const inProgressCourses = enrollments.filter(e => e.progress > 0 && e.progress < 100);
  const completedCourses = enrollments.filter(e => e.progress === 100);
  const notStartedCourses = enrollments.filter(e => e.progress === 0);

  const totalProgress = enrollments.length > 0
    ? Math.round(enrollments.reduce((sum, e) => sum + e.progress, 0) / enrollments.length)
    : 0;

  const stats = [
    {
      icon: BookOpen,
      label: 'Cursos Inscritos',
      value: enrollments.length,
      color: 'bg-blue-500',
    },
    {
      icon: TrendingUp,
      label: 'Em Progresso',
      value: inProgressCourses.length,
      color: 'bg-yellow-500',
    },
    {
      icon: Award,
      label: 'Concluídos',
      value: completedCourses.length,
      color: 'bg-green-500',
    },
    {
      icon: Clock,
      label: 'Progresso Médio',
      value: `${totalProgress}%`,
      color: 'bg-red-500',
    },
  ];

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-gray-900">Meu Aprendizado</h1>
        <p className="text-gray-600 mt-1">Acompanhe seu progresso e continue aprendendo</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, index) => (
          <div
            key={index}
            className="bg-white rounded-xl shadow-sm p-6 hover:shadow-md transition"
          >
            <div className={`w-12 h-12 ${stat.color} rounded-lg flex items-center justify-center mb-4`}>
              <stat.icon className="w-6 h-6 text-white" />
            </div>
            <p className="text-3xl font-bold text-gray-900 mb-1">{stat.value}</p>
            <p className="text-gray-600">{stat.label}</p>
          </div>
        ))}
      </div>

      {enrollments.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-12 text-center">
          <BookOpen className="w-16 h-16 text-gray-400 mx-auto mb-4" />
          <h3 className="text-xl font-bold text-gray-900 mb-2">
            Você ainda não está inscrito em nenhum curso
          </h3>
          <p className="text-gray-600 mb-6">
            Explore nosso catálogo e comece sua jornada de aprendizado hoje!
          </p>
        </div>
      ) : (
        <>
          {inProgressCourses.length > 0 && (
            <div>
              <h2 className="text-2xl font-bold text-gray-900 mb-4">Continuar Aprendendo</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {inProgressCourses.map(enrollment => (
                  <CourseCard
                    key={enrollment.id}
                    course={enrollment.course}
                    onViewDetails={onViewCourseDetails}
                    enrolled={true}
                    progress={enrollment.progress}
                  />
                ))}
              </div>
            </div>
          )}

          {notStartedCourses.length > 0 && (
            <div>
              <h2 className="text-2xl font-bold text-gray-900 mb-4">Cursos para Começar</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {notStartedCourses.map(enrollment => (
                  <CourseCard
                    key={enrollment.id}
                    course={enrollment.course}
                    onViewDetails={onViewCourseDetails}
                    enrolled={true}
                    progress={enrollment.progress}
                  />
                ))}
              </div>
            </div>
          )}

          {completedCourses.length > 0 && (
            <div>
              <h2 className="text-2xl font-bold text-gray-900 mb-4">Cursos Concluídos</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {completedCourses.map(enrollment => (
                  <CourseCard
                    key={enrollment.id}
                    course={enrollment.course}
                    onViewDetails={onViewCourseDetails}
                    enrolled={true}
                    progress={enrollment.progress}
                  />
                ))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
};
